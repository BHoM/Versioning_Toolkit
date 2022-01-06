/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

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

        public void ProcessingLoop(string pipeName, Converter converter)
        {
            // Make sure all assemblies are loaded
            AssemblyName[] assemblies = Assembly.GetEntryAssembly().GetReferencedAssemblies();
            foreach (AssemblyName assembly in assemblies)
            {
                Assembly.Load(assembly);
                Console.WriteLine("Assembly Loaded: " + assembly.Name);
            }
            Console.WriteLine("");

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
                    BsonDocument newDoc = null;
                    try
                    {
                        BsonDocument doc = ReadDocument(reader);
                        if (doc != null)
                            Console.WriteLine("document received: " + doc.ToJson());
                        newDoc = Upgrade(doc, converter);
                    }
                    catch (NoUpdateException e)
                    {
                        newDoc = new BsonDocument
                        {
                            { "_t", "NoUpdate" },
                            { "Message", e.Message }
                        };
                    }
                    catch { }
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

        private BsonDocument Upgrade(BsonDocument document, Converter converter)
        {
            if (document == null)
                return null;

            Console.WriteLine("document received: " + document.ToJson());

            BsonDocument result = null;
            if (document.Contains("_t"))
            {
                if (document["_t"] == "System.Reflection.MethodBase")
                    result = UpgradeMethod(document, converter);
                else if (document["_t"] == "System.Type")
                    result = UpgradeType(document, converter);
                else
                    result = UpgradeObject(document, converter);
            }
                
            return result;
        }

        /***************************************************/

        private BsonDocument UpgradeMethod(BsonDocument document, Converter converter)
        {
            BsonValue typeName = document["TypeName"];
            BsonValue methodName = document["MethodName"];
            BsonArray parameterArray = document["Parameters"] as BsonArray;
            if (typeName == null || methodName == null || parameterArray == null)
                return null;

            // Get the method key
            string key = GetMethodKey(document);
            if (string.IsNullOrEmpty(key))
                return null;

            // First see if the method can be found in the dictionary using the old parameters
            if (converter.ToNewMethod.ContainsKey(key))
                return converter.ToNewMethod[key];

            // Check if the method is classified as deleted or without update
            CheckForNoUpgrade(converter, key, "method");

            // Update the parameter types if needed
            bool modified = false;
            List<BsonValue> parameters = new List<BsonValue>();
            foreach (BsonValue parameter in parameterArray)
            {
                string newParam = UpgradeType(parameter.AsString, converter);
                if (newParam != null)
                {
                    modified = true;
                    parameters.Add(newParam);
                }
                else
                    parameters.Add(parameter.AsString);
            }

            // Update the declaring type if needed
            string newDeclaringType = UpgradeType(typeName.AsString, converter);
            if (newDeclaringType != null)
            {
                typeName = newDeclaringType;
                modified = true;
            }

            if (modified)
            {
                document = new BsonDocument(new Dictionary<string, object>
                {
                    { "_t", "System.Reflection.MethodBase" },
                    { "TypeName", typeName },
                    { "MethodName", methodName.AsString },
                    { "Parameters", parameters }
                });
                key = GetMethodKey(document);

                if (converter.ToNewMethod.ContainsKey(key))
                    return converter.ToNewMethod[key];
                else
                    return document;
            }

            // No match found -> return null
            return null;
        }

        /***************************************************/

        private BsonDocument UpgradeType(BsonDocument document, Converter converter)
        {
            if (document == null || !document.Contains("Name"))
                return null;

            // Check if the type is classified as deleted or without update
            string type = document["Name"].AsString;
            CheckForNoUpgrade(converter, type.Split(',').First(), "type");

            // Then check if the type can be upgraded
            bool modified = false;
            string typeFromDic = GetTypeFromDic(converter.ToNewType, type);
            if (typeFromDic != null)
            {
                document["Name"] = typeFromDic;
                modified = true;
            }
                
            if (document.Contains("GenericArguments"))
            {
                BsonArray newGenerics = new BsonArray();
                BsonArray generics = document["GenericArguments"].AsBsonArray;
                if (generics != null)
                {
                    foreach (BsonDocument generic in generics)
                    {
                        BsonDocument newGeneric = Upgrade(generic, converter);
                        if (newGeneric != null)
                        {
                            modified = true;
                            newGenerics.Add(newGeneric);
                        }  
                        else
                            newGenerics.Add(generic);
                    }
                }
                document["GenericArguments"] = newGenerics;
            }

            if (modified)
                return document;
            else
                return null;
        }

        /***************************************************/

        private string UpgradeType(string type, Converter converter)
        {
            BsonDocument doc = null;
            BsonDocument.TryParse(type, out doc);
            BsonValue newType = Upgrade(doc, converter) as BsonValue;
            if (newType == null)
                return null;
            else
            {
                string newString = newType.ToString();
                if (newString.Length == 0)
                    return null;
                else
                    return newString;
            }
        }

        /***************************************************/

        private BsonDocument UpgradeObject(BsonDocument document, Converter converter)
        {
            //Get the old type
            string oldType = document["_t"].AsString.Split(',').First();

            // Check if the object type is classified as deleted or without update
            CheckForNoUpgrade(converter, oldType, "object type");

            // Upgrade porperties
            BsonDocument withNewProperties = UpgradeObjectProperties(document, converter);
            BsonDocument newDoc = withNewProperties ?? document;

            //Check if there are any full object upgraders available
            if (converter.ToNewObject.ContainsKey(oldType))
            {
                //If so, use them to upgrade the object
                newDoc = UpgradeObjectExplicit(newDoc, converter, oldType) ?? newDoc;
            }
            else
            {
                //If not, try upgrading the names of the types and properties
                newDoc = UpgradeObjectTypeAndPropertyNames(newDoc, converter, oldType) ?? newDoc;
            }

            return newDoc;
        }

        /***************************************************/

        private BsonArray UpgradeArray(BsonArray array, Converter converter, out bool upgraded)
        {
            upgraded = false;
            if (array == null)
                return null;

            BsonArray newArray = new BsonArray();
            foreach (BsonValue item in array)
            {
                if (item is BsonDocument)
                {
                    BsonDocument doc = item as BsonDocument;
                    BsonDocument upgrade = Upgrade(doc, converter);
                    if (upgrade == null)
                        upgrade = doc;
                    else if (upgrade != doc)
                        upgraded = true;
                    newArray.Add(upgrade);
                }
                else if (item is BsonArray)
                {
                    bool itemUpgraded = false;
                    newArray.Add(UpgradeArray(item as BsonArray, converter, out itemUpgraded));
                    if (itemUpgraded)
                        upgraded = true;
                }
            }

            return newArray;
        }

        /***************************************************/

        private BsonDocument UpgradeObjectProperties(BsonDocument document, Converter converter)
        {
            Dictionary<string, BsonValue> propertiesToUpgrade = new Dictionary<string, BsonValue>();
            foreach (BsonElement property in document)
            {
                string propName = property.Name;

                if (property.Value is BsonDocument)
                {
                    BsonDocument prop = property.Value as BsonDocument;
                    BsonDocument upgrade = Upgrade(prop, converter);
                    if (upgrade != null && prop != upgrade)
                        propertiesToUpgrade.Add(propName, upgrade);
                }
                else if (property.Value is BsonArray)
                {
                    bool upgraded = false;
                    BsonArray newArray = UpgradeArray(property.Value as BsonArray, converter, out upgraded);
                    if (upgraded)
                        propertiesToUpgrade.Add(propName, newArray);
                }
            }

            foreach (var kvp in propertiesToUpgrade)
                document[kvp.Key] = kvp.Value;

            if (propertiesToUpgrade.Count > 0)
                return document;
            else
                return null;
        }

        /***************************************************/

        private BsonDocument UpgradeObjectExplicit(BsonDocument document, Converter converter, string oldType)
        {
            try
            {
                Dictionary<string, object> b = converter.ToNewObject[oldType](document.ToDictionary());
                if (b == null)
                    return null;

                Console.WriteLine("object updated: " + b.GetType().FullName);
                BsonDocument newDoc = new BsonDocument(b);

                // Copy over BHoM properties
                string[] properties = new string[] { "BHoM_Guid", "CustomData", "Name", "Tags", "Fragments" };
                foreach (string p in properties)
                {
                    if (newDoc.Contains(p) && document.Contains(p))
                        newDoc[p] = document[p];
                }

                return newDoc;
            }
            catch
            {
                return null;
            }
        }

        /***************************************************/

        private BsonDocument UpgradeObjectTypeAndPropertyNames(BsonDocument document, Converter converter, string oldType)
        {
            // Upgrade the property names
            Dictionary<string, BsonElement> propertiesToRename = new Dictionary<string, BsonElement>();
            foreach (BsonElement property in document)
            {
                string propName = property.Name;
                string key = oldType + "." + propName;
                if (converter.ToNewProperty.ContainsKey(key))
                {
                    propName = converter.ToNewProperty[key].Split('.').Last();
                    propertiesToRename.Add(property.Name, new BsonElement(propName, property.Value));
                }
            }

            foreach (var kvp in propertiesToRename)
            {
                document.Add(kvp.Value);
                document.Remove(kvp.Key);
            }

            //Try to find new type
            string newType = GetTypeFromDic(converter.ToNewType, oldType);
            if (newType != null)
            {
                document["_t"] = newType;
                Console.WriteLine("type upgraded from " + oldType + " to " + newType);
                return document;
            }
            else if (propertiesToRename.Count > 0)
                return document;
            else
                return null;
        }

        /***************************************************/

        private static string GetTypeFromDic(Dictionary<string, string> dic, string type, bool acceptPartialNamespace = true)
        {
            if (type.Contains(","))
                type = type.Split(',').First();

            if (dic.ContainsKey(type))
                return dic[type];
            else if (acceptPartialNamespace)
            {
                int index = type.LastIndexOf('.');
                while (index > 0)
                {
                    string ns = type.Substring(0, index);
                    if (dic.ContainsKey(ns))
                        return dic[ns] + type.Substring(index);
                    else
                        index = ns.LastIndexOf('.');
                }
            }

            return null;
        }

        /***************************************************/

        private static void CheckForNoUpgrade(Converter converter, string key, string itemTypeName)
        {
            // Check if the method is classified as deleted or without update
            string message = "";
            if (converter.MessageForDeleted.ContainsKey(key))
                message = $"No upgrade for {key}. This {itemTypeName} has been deleted without replacement.\n" + converter.MessageForDeleted[key];
            else if (converter.MessageForNoUpgrade.ContainsKey(key))
                message = $"No upgrade for {key}. This {itemTypeName} cannot be upgraded automatically.\n" + converter.MessageForNoUpgrade[key];

            if (!string.IsNullOrWhiteSpace(message))
                throw new NoUpdateException(message);
        }

        /***************************************************/

        private static string GetMethodKey(BsonDocument method)
        {
            BsonValue typeName = method["TypeName"];
            BsonValue methodName = method["MethodName"];
            BsonArray parameters = method["Parameters"] as BsonArray;
            if (typeName == null || methodName == null || parameters == null)
                return null;

            string name = methodName.ToString();
            if (name == ".ctor")
                name = "";
            else
                name = "." + name;

            string declaringType = GetTypeString(typeName.AsString);

            string parametersString = "";
            List<string> parameterTypes = parameters.Select(x => GetTypeString(x.AsString)).ToList();
            if (parameterTypes.Count > 0)
                parametersString = parameterTypes.Aggregate((a, b) => a + ", " + b);

            return declaringType + name + "(" + parametersString + ")";
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

            if (!doc.Contains("GenericArguments"))
                return typeString;

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
    }
}


