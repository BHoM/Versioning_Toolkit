/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

namespace BH.Upgrader.v60
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "5.3";

            ToNewObject.Add("BH.oM.Adapters.Revit.RevitMaterialTakeOff", UpgradeRevitMaterialTakeoff);
            ToNewObject.Add("BH.oM.LadybugTools.ExternalComfortShelter", UpgradeShelter);
            ToNewObject.Add("BH.oM.LadybugTools.ExternalComfortTypology", UpgradeTypology);
            ToNewObject.Add("BH.oM.XML.Bluebeam.Markup", UpdateBluebeamMarkdown);
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        public static Dictionary<string, object> UpgradeRevitMaterialTakeoff(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.oM.Physical.Materials.VolumetricMaterialTakeoff";

            double totalVolume = 0;
            object volumeObj;
            if (oldVersion.TryGetValue("TotalVolume", out volumeObj) && volumeObj is double)
                totalVolume = (double)volumeObj;

            List<object> materials = new List<object>();
            List<double> volumes = new List<double>();
            object matCompObj;
            if (oldVersion.TryGetValue("MaterialComposition", out matCompObj))
            {
                Dictionary<string, object> matComp = matCompObj as Dictionary<string, object>;
                if (matComp != null)
                {
                    
                    object materialsObj;
                    if(matComp.TryGetValue("Materials", out materialsObj))
                        materials = (materialsObj as IEnumerable<object>)?.ToList();

                    List<double> ratios = null;
                    object ratiosObj;

                    if (matComp.TryGetValue("Ratios", out ratiosObj))
                    {
                        List<object> list = (ratiosObj as IEnumerable<object>)?.ToList();
                        if (list != null)
                            ratios = list.Cast<double>().ToList();
                    }

                    if (ratios != null)
                    {
                        foreach (double ratio in ratios)
                        {
                            volumes.Add(totalVolume * ratio);
                        }
                    }
                }
            }

            newVersion["Materials"] = materials;
            newVersion["Volumes"] = volumes;
            return newVersion;

        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeShelter(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.oM.LadybugTools.Shelter";

            newVersion["WindPorosity"] = oldVersion["Porosity"];
            newVersion["RadiationPorosity"] = oldVersion["Porosity"];
            newVersion["AzimuthRange"] = new List<double>() { 
                (double)oldVersion["StartAzimuth"], 
                (double)oldVersion["EndAzimuth"] 
            };
            newVersion["AltitudeRange"] = new List<double>() {
                (double)oldVersion["StartAltitude"],
                (double)oldVersion["EndAltitude"]
            };

            return newVersion;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeTypology(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.oM.LadybugTools.Typology";

            newVersion["Name"] = oldVersion["Name"];
            newVersion["Shelters"] = oldVersion["Shelters"];
            newVersion["EvaporativeCoolingEffectiveness"] = oldVersion["EvaporativeCoolingEffectiveness"];
            newVersion["WindSpeedAdjustment"] = 0;

            return newVersion;
        }

        /***************************************************/

        public static Dictionary<string, object> UpdateBluebeamMarkdown(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            newVersion["Depth"] = oldVersion["Depth"].ToString();

            return newVersion;
        }

        /***************************************************/
    }
}


