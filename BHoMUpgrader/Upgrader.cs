using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgrader.Base
{
    public class Upgrader
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        public void ProcessingLoop(string pipeName, IConverter converter)
        {
            // Make sure all assemblies are loaded
            System.Reflection.AssemblyName[] assemblies = System.Reflection.Assembly.GetEntryAssembly().GetReferencedAssemblies();
            foreach (System.Reflection.AssemblyName assembly in assemblies)
            {
                System.Reflection.Assembly.Load(assembly);
                Console.WriteLine("Assembly Loaded: " + assembly.Name);
            }
            Console.WriteLine("");

            // Gather all the conversion methods from the converter
            MethodInfo[] converterMethods = converter.GetType().GetMethods();
            foreach (MethodInfo method in converterMethods.Where(x => x.Name == "ToNew"))
            {
                ParameterInfo parameter = method.GetParameters().FirstOrDefault();
                if (parameter != null)
                {
                    switch (method.Name)
                    {
                        case "ToNew":
                            m_ToNewConverters.Add(parameter.ParameterType.FullName);
                            break;
                        case "ToOld":
                            m_ToOldConverters.Add(parameter.ParameterType.FullName);
                            break;
                    }
                }       
            }

            // Deactivate the upgrade check in teh serialiser
            BH.Engine.Serialiser.Compute.AllowUpgradeFromBson(false);

            NamedPipeClientStream pipe = null;

            BinaryWriter writer = null;
            BinaryReader reader = null;

            try
            {
                pipe = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut);
                pipe.Connect();

                writer = new BinaryWriter(pipe);
                reader = new BinaryReader(pipe);

                while (pipe.IsConnected)
                {
                    BsonDocument doc = ReadDocument(reader);
                    BsonDocument newDoc = Upgrade(doc, converter);
                    SendDocument(newDoc, writer);
                }
            }
            finally
            {
                if (pipe != null)
                    pipe.Dispose();
            }
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private BsonDocument Upgrade(BsonDocument document, IConverter converter)
        {
            if (document == null)
                return null;

            Console.WriteLine("document received: " + document.ToJson());

            if (document.Contains("_t"))
            {
                string oldType = document["_t"].AsString;
                string newType = ToNewType(oldType, converter);
                document["_t"] = newType;
                if (newType != oldType)
                {
                    Console.WriteLine("type upgraded from " + oldType + " to " + newType);
                    return document;
                }  
                else if (m_ToNewConverters.Contains(oldType))
                {
                    object item = BH.Engine.Serialiser.Convert.FromBson(document);
                    if (item == null)
                        return null;

                    Console.WriteLine("object recovered: " + item.GetType().FullName);

                    object b = converter.IToNew(item as dynamic);
                    if (b == null)
                        return null;

                    Console.WriteLine("object updated: " + b.GetType().FullName);
                    BsonDocument newDoc = BH.Engine.Serialiser.Convert.ToBson(b);

                    // Copy over BHoM properties
                    string[] properties = new string[] { "BHoM_Guid", "CustomData", "Name", "Tags", "Fragments" };
                    foreach (string p in properties)
                    {
                        if (newDoc.Contains(p) && document.Contains(p))
                            newDoc[p] = document[p];
                    }

                    return newDoc;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /***************************************************/

        private string ToNewType(string type, IConverter converter)
        {
            return GetTypeFromDic(converter.ToNewType, type);
        }

        /***************************************************/

        private string ToOldType(string type, IConverter converter)
        {
            return GetTypeFromDic(converter.ToOldType, type);
        }

        /***************************************************/

        private static string GetTypeFromDic(Dictionary<string, string> dic, string type)
        {
            if (dic.ContainsKey(type))
                return dic[type];
            else
            {
                int index = type.LastIndexOf('.');
                if (index > 0)
                {
                    string ns = type.Substring(0, index);
                    if (dic.ContainsKey(ns))
                        return dic[ns] + type.Substring(index);
                    else
                        return type;
                }
                else
                    return type;
            }
        }

        /***************************************************/

        private static void SendDocument(BsonDocument document, BinaryWriter writer)
        {
            try
            {
                if (document == null)
                    writer.Write(0);

                MemoryStream memory = new MemoryStream();
                BsonBinaryWriter memoryWriter = new BsonBinaryWriter(memory);
                BsonSerializer.Serialize(memoryWriter, typeof(BsonDocument), document);
                byte[] content = memory.ToArray();

                writer.Write(content.Length);

                writer.Write(content);
                writer.Flush();

                memory.Dispose();
            }
            catch { }
        }

        /***************************************************/

        private static BsonDocument ReadDocument(BinaryReader reader)
        {
            try
            {
                int contentSize = reader.ReadInt32();
                if (contentSize == 0)
                    return null;

                byte[] content = new byte[contentSize];
                reader.Read(content, 0, contentSize);

                return BsonSerializer.Deserialize(content, typeof(BsonDocument)) as BsonDocument;
            }
            catch
            {
                return null;
            }
        }


        /***************************************************/
        /**** Private Fields                            ****/
        /***************************************************/

        private HashSet<string> m_ToNewConverters = new HashSet<string>();
        private HashSet<string> m_ToOldConverters = new HashSet<string>();

        /***************************************************/
    }
}
