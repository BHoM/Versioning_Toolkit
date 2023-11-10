/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BH.Upgrader.v70
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "6.3";
            ToNewObject.Add("BH.oM.LadybugTools.OpaqueMaterial", UpgradeILadybugToolsMaterial);
            ToNewObject.Add("BH.oM.LadybugTools.OpaqueVegetationMaterial", UpgradeILadybugToolsMaterial);
            ToNewObject.Add("BH.oM.LadybugTools.ILadybugToolsMaterial", UpgradeILadybugToolsMaterial);
            ToNewObject.Add("BH.oM.LadybugTools.Shelter", UpgradeShelter);
            ToNewObject.Add("BH.oM.LadybugTools.ExternalComfort", UpgradeExternalComfort);
            ToNewObject.Add("BH.oM.LadybugTools.SimulationResult", UpgradeSimulationResult);
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, object> UpgradeShelter(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            List<double> numbers;

            if (newVersion.TryGetValue("WindPorosity", out object wind))
            {
                numbers = Enumerable.Repeat((double)wind, 8760).ToList();
                newVersion["WindPorosity"] = numbers;
            }
            if (newVersion.TryGetValue("RadiationPorosity", out object radiation))
            {
                numbers = Enumerable.Repeat((double)radiation, 8760).ToList();
                newVersion["RadiationPorosity"] = numbers;
            }

            return newVersion;
        }

        private static Dictionary<string, object> UpgradeILadybugToolsMaterial(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (oldVersion.ContainsKey("_t"))
            {
                if ((string)oldVersion["_t"] == "BH.oM.LadybugTools.OpaqueMaterial")
                {
                    newVersion["_t"] = "BH.oM.LadybugTools.EnergyMaterial";
                }
                if ((string)oldVersion["_t"] == "BH.oM.LadybugTools.OpaqueVegetationMaterial")
                {
                    newVersion["_t"] = "BH.oM.LadybugTools.EnergyMaterialVegetation";
                }
                if ((string)oldVersion["_t"] == "BH.oM.LadybugTools.ILadybugToolsMaterial")
                {
                    newVersion["_t"] = "BH.oM.LadybugTools.IEnergyMaterialOpaque";
                }
            }

            if (newVersion.ContainsKey("Source"))
            {
                newVersion["Source"] = null;
                newVersion.Remove("Source");
            }

            return newVersion;
        }

        private static Dictionary<string, object> HourlyContinuousCollection(Dictionary<string, object> CustomData)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();

            newVersion.Add("_t", "BH.oM.LadybugTools.HourlyContinuousCollection");
            newVersion.Add("Type", CustomData["Type"]);
            newVersion.Add("Header", CustomData["Header"]);
            newVersion.Add("Values", CustomData["Values"]);

            Dictionary<string, object> Header = newVersion["Header"] as Dictionary<string, object>;
            Header.Add("_t", "BH.oM.LadybugTools.Header");

            (Header["Datatype"] as Dictionary<string, object>).Add("_t", "BH.oM.LadybugTools.DataType");
            (Header["AnalysisPeriod"] as Dictionary<string, object>).Add("_t", "BH.oM.LadybugTools.AnalysisPeriod");

            newVersion["Header"] = Header;
            return newVersion;
        }

        //So much stuff to version!
        private static Dictionary<string, object> UpgradeExternalComfort(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            Dictionary<string, object> CustomData;
            List<string> keys = new List<string>()
            {
                "UniversalThermalClimate",
                "DryBulbTemperature",
                "RelativeHumidity",
                "WindSpeed",
                "MeanRadiantTemperature",
            };

            if (oldVersion.ContainsKey("CustomData"))
            {
                CustomData = oldVersion["CustomData"] as Dictionary<string, object>;
                (newVersion["CustomData"] as Dictionary<string, object>).Clear();
                foreach (string key in keys)
                {
                    if (CustomData.ContainsKey(key))
                    {
                        newVersion.Add(key, HourlyContinuousCollection(CustomData[key] as Dictionary<string, object>));
                    }
                }
            }

            return newVersion;
        }

        private static Dictionary<string, object> UpgradeSimulationResult(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            Dictionary<string, object> CustomData;
            List<string> keys = new List<string>()
            {
                "ShadedDownTemperature",
                "ShadedUpTemperature",
                "ShadedRadiantTemperature",
                "ShadedLongwaveMeanRadiantTemperatureDelta",
                "ShadedShortwaveMeanRadiantTemperatureDelta",
                "ShadedMeanRadiantTemperature",
                "UnshadedDownTemperature",
                "UnshadedUpTemperature",
                "UnshadedRadiantTemperature",
                "UnshadedLongwaveMeanRadiantTemperatureDelta",
                "UnshadedShortwaveMeanRadiantTemperatureDelta",
                "UnshadedMeanRadiantTemperature"
            };

            if (oldVersion.ContainsKey("CustomData"))
            {
                CustomData = oldVersion["CustomData"] as Dictionary<string, object>;
                (newVersion["CustomData"] as Dictionary<string, object>).Clear();
                foreach (string key in keys)
                {
                    if (CustomData.ContainsKey(key))
                    {
                        newVersion.Add(key, HourlyContinuousCollection(CustomData[key] as Dictionary<string, object>));
                    }
                }
            }

            if (oldVersion.ContainsKey("GroundMaterial"))
            {
                newVersion["GroundMaterial"] = UpgradeILadybugToolsMaterial(oldVersion["GroundMaterial"] as Dictionary<string, object>);
            }
            if (oldVersion.ContainsKey("ShadeMaterial"))
            {
                newVersion["ShadeMaterial"] = UpgradeILadybugToolsMaterial(oldVersion["ShadeMaterial"] as Dictionary<string, object>);
            }

            return newVersion;
        }
    }
}
