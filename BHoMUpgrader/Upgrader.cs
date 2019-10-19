using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
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

        /***************************************************/

        private static void SendDocument(BsonDocument document, BinaryWriter writer)
        {
            try
            {
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
    }
}
