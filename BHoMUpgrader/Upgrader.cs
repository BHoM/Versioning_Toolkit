using BH.Engine.Reflection;
using BH.oM.Base;
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
            AssemblyName[] assemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            foreach (AssemblyName assembly in assemblies)
            {
                Assembly.Load(assembly);
                Console.WriteLine("Assembly Loaded: " + assembly.Name);
            }
            Console.WriteLine("");

            // Gather all the conversion methods from the converter
            MethodInfo[] converterMethods = converter.GetType().GetMethods();
            foreach (MethodInfo method in converterMethods)
            {
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 1)
                {
                    switch (method.Name)
                    {
                        case "ToNew":
                            m_ToNewFromDeprecatedConverters.Add(parameters[0].ParameterType.FullName);
                            break;
                        case "ToOld":
                            m_ToOldDeprecatedConverters.Add(parameters[0].ParameterType.FullName);
                            break;
                    }
                }
                else if (parameters.Length == 2)
                {
                    if (parameters[0].ParameterType == typeof(Dictionary<string, object>) && method.Name == "ToNew")
                        m_ToNewFromCustomConverters.Add(parameters[1].ParameterType.FullName, parameters[1].ParameterType);
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
                if (document["_t"] == "System.Reflection.MethodBase")
                    return UpgradeMethod(document, converter);
                else
                    return UpgradeObject(document, converter);
            }
            else
            {
                return null;
            }
        }

        /***************************************************/

        private BsonDocument UpgradeMethod(BsonDocument document, IConverter converter)
        {
            return ToNewMethod(document, converter);
        }

        /***************************************************/

        private BsonDocument UpgradeObject(BsonDocument document, IConverter converter)
        {
            string oldType = document["_t"].AsString;
            string newType = ToNewType(oldType, converter);
            document["_t"] = newType;
            if (newType != oldType)
            {
                Console.WriteLine("type upgraded from " + oldType + " to " + newType);
                return document;
            }
            else if (m_ToNewFromDeprecatedConverters.Contains(oldType))
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
            else if (m_ToNewFromCustomConverters.ContainsKey(oldType))
            {
                object instance = Activator.CreateInstance(m_ToNewFromCustomConverters[oldType]);
                object b = converter.IToNew(document.ToDictionary(), instance);
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

        private BsonDocument ToNewMethod(BsonDocument method, IConverter converter)
        {
            return GetMethodFromDic(converter.ToNewMethod, method);
        }

        /***************************************************/

        private BsonDocument ToOldMethod(BsonDocument method, IConverter converter)
        {
            return GetMethodFromDic(converter.ToOldMethod, method);
        }

        /***************************************************/

        private static BsonDocument GetMethodFromDic(Dictionary<string, MethodBase> dic, BsonDocument method)
        {
            BsonValue typeName = method["TypeName"];
            BsonValue methodName = method["MethodName"];
            BsonArray parameters = method["Parameters"] as BsonArray;
            if (typeName == null || methodName == null || parameters == null)
                return method;

            string name = methodName.ToString();
            List<string> parameterTypes = parameters.Select(x => GetTypeString(x.AsString)).ToList();
            string declaringType = GetTypeString(typeName.AsString);

            string parametersString = "";
            if (parameterTypes.Count > 0)
                parametersString = parameterTypes.Aggregate((a, b) => a + ", " + b);
            string key = declaringType + "." + name + "(" + parametersString + ")";

            if (dic.ContainsKey(key))
                return Engine.Serialiser.Convert.ToBson(dic[key]);
            else
                return method;
        }

        /***************************************************/

        private static string GetTypeString(string json)
        {
            // The type stored in json might not exist anywhere anymore so we have to go old school (i.e. no FromJson(json).ToText())
            string typeString = "";

            BsonDocument doc;
            if (BsonDocument.TryParse(json, out doc))
            {
                BsonValue name = doc["Name"];
                if (name == null)
                    return "";
                typeString = name.ToString();
                int cut = typeString.IndexOf(',');
                if (cut > 0)
                    typeString = typeString.Substring(0, cut);
            }
            else
                return "";

            int genericIndex = typeString.IndexOf('`');
            if (genericIndex < 0)
                return typeString;
            typeString = typeString.Substring(0, genericIndex);

            BsonArray generics = doc["GenericArguments"] as BsonArray;
            if (generics == null)
                return typeString;

            string genericString = generics.Select(x => GetTypeString(x.ToString())).Aggregate((a, b) => a + ", " + b);
            return typeString + "<" + genericString + ">";
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

        private HashSet<string> m_ToNewFromDeprecatedConverters = new HashSet<string>();
        private HashSet<string> m_ToOldDeprecatedConverters = new HashSet<string>();
        private Dictionary<string, Type> m_ToNewFromCustomConverters = new Dictionary<string, Type>();

        /***************************************************/
    }
}
