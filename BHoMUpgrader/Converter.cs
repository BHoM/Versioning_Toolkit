/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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

using BH.oM.Base;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgrader.Base
{
    public delegate Dictionary<string, object> CustomUpgrader(Dictionary<string, object> item);


    public class Converter
    {
        /***************************************************/
        /**** Public Properties                         ****/
        /***************************************************/

        public string PreviousVersion { get; set; } = "";

        public Dictionary<string, string> ToNewType { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> ToOldType { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> ToNewProperty { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> ToOldProperty { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, BsonDocument> ToNewMethod { get; set; } = new Dictionary<string, BsonDocument>();

        public Dictionary<string, BsonDocument> ToOldMethod { get; set; } = new Dictionary<string, BsonDocument>();

        public Dictionary<string, CustomUpgrader> ToNewObject { get; set; } = new Dictionary<string, CustomUpgrader>();

        public Dictionary<string, CustomUpgrader> ToOldObject { get; set; } = new Dictionary<string, CustomUpgrader>();

        public Dictionary<string, string> MessageForDeleted { get; set; } = new Dictionary<string, string>();

        public Dictionary<string, string> MessageForNoUpgrade { get; set; } = new Dictionary<string, string>();


        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter()
        {
            string folder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            LoadUpgrades(Path.Combine(folder, "Upgrades.json"));
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        protected void LoadUpgrades(string upgradesFile)
        {
            if (!File.Exists(upgradesFile))
            {
                Console.WriteLine("Failed to find the upgrade file: " + upgradesFile);
                return;
            }

            string json = File.ReadAllText(upgradesFile);
            BsonDocument upgrades = null;
            if (!BsonDocument.TryParse(json, out upgrades))
            {
                Console.WriteLine("Failed to load the upgrade file.");
                return;
            }

            if (upgrades.Contains("Type"))
                LoadTypeUpgrades(upgrades["Type"] as BsonDocument);

            if (upgrades.Contains("Namespace"))
                AddNamespaceUpgrades(upgrades["Namespace"] as BsonDocument);

            if (upgrades.Contains("Method"))
                LoadMethodUpgrades(upgrades["Method"] as BsonDocument);

            if (upgrades.Contains("Property"))
                LoadPropertyUpgrades(upgrades["Property"] as BsonDocument);

            if (upgrades.Contains("MessageForDeleted"))
                LoadMessageForDeleted(upgrades["MessageForDeleted"] as BsonDocument);

            if (upgrades.Contains("MessageForNoUpgrade"))
                LoadMessageForNoUpgrade(upgrades["MessageForNoUpgrade"] as BsonDocument);
        }

        /***************************************************/

        protected void LoadTypeUpgrades(BsonDocument data)
        {
            if (data == null)
                return;

            if (data.Contains("ToNew"))
            {
                BsonDocument toNew = data["ToNew"] as BsonDocument;
                if (toNew != null)
                    ToNewType = toNew.ToDictionary(x => x.Name, x => x.Value.AsString);
            }
                
            if (data.Contains("ToOld"))
            {
                BsonDocument toOld = data["ToOld"] as BsonDocument;
                if (toOld != null)
                    ToOldType = toOld.ToDictionary(x => x.Name, x => x.Value.ToString());
            }
        }

        /***************************************************/

        protected void AddNamespaceUpgrades(BsonDocument data)
        {
            if (data == null)
                return;

            if (data.Contains("ToNew"))
            {
                BsonDocument toNew = data["ToNew"] as BsonDocument;
                if (toNew != null)
                {
                    foreach (BsonElement element in toNew)
                        ToNewType.Add(element.Name, element.Value.AsString);
                }
            }

            if (data.Contains("ToOld"))
            {
                BsonDocument toOld = data["ToOld"] as BsonDocument;
                if (toOld != null)
                {
                    foreach (BsonElement element in toOld)
                        ToOldType.Add(element.Name, element.Value.AsString);
                }
            }
        }

        /***************************************************/

        protected void LoadMethodUpgrades(BsonDocument data)
        {
            if (data.Contains("ToNew"))
            {
                BsonDocument toNew = data["ToNew"] as BsonDocument;
                if (toNew != null)
                    ToNewMethod = toNew.ToDictionary(x => x.Name, x => x.Value.ToBsonDocument());
            }

            if (data.Contains("ToOld"))
            {
                BsonDocument toOld = data["ToOld"] as BsonDocument;
                if (toOld != null)
                    ToOldMethod = toOld.ToDictionary(x => x.Name, x => x.Value.ToBsonDocument());
            }
        }

        /***************************************************/

        protected void LoadPropertyUpgrades(BsonDocument data)
        {
            if (data == null)
                return;

            if (data.Contains("ToNew"))
            {
                BsonDocument toNew = data["ToNew"] as BsonDocument;
                if (toNew != null)
                    ToNewProperty = toNew.ToDictionary(x => x.Name, x => x.Value.AsString);
            }

            if (data.Contains("ToOld"))
            {
                BsonDocument toOld = data["ToOld"] as BsonDocument;
                if (toOld != null)
                    ToOldProperty = toOld.ToDictionary(x => x.Name, x => x.Value.ToString());
            }
        }

        /***************************************************/

        protected void LoadMessageForDeleted(BsonDocument data)
        {
            if (data != null)
                MessageForDeleted = data.ToDictionary(x => x.Name, x => x.Value.AsString);
        }

        /***************************************************/

        protected void LoadMessageForNoUpgrade(BsonDocument data)
        {
            if (data != null)
                MessageForNoUpgrade = data.ToDictionary(x => x.Name, x => x.Value.AsString);
        }

        /***************************************************/
    }
}




