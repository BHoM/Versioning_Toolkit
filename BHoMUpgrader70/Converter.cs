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
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, object> UpgradeOpaqueMaterial(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            newVersion["_t"] = "BH.oM.LadybugTools.EnergyMaterial";

            if (newVersion.ContainsKey("Source"))
            {
                newVersion["Source"] = null;
                newVersion.Remove("Source");
            }

            return newVersion;
        }

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
            newVersion["_t"] = "BH.oM.LadybugTools.IEnergyMaterialOpaque";

            if (newVersion.ContainsKey("Source"))
            {
                newVersion["Source"] = null;
                newVersion.Remove("Source");
            }

            return newVersion;
        }

        private static Dictionary<string, object> UpgradeSimulationResult(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            Dictionary<string, object> Material;

            if (oldVersion.TryGetValue("GroundMaterial", out object ground))
            {
                Material = ground as Dictionary<string, object>;
                if (Material.ContainsKey("_t"))
                {
                    Material["_t"] = "BH.oM.LadybugTools.IEnergyMaterialOpaque";
                    newVersion["GroundMaterial"] = Material;
                }
            }
            if (oldVersion.TryGetValue("ShadeMaterial", out object shade))
            {
                Material = shade as Dictionary<string, object>;
                if (Material.ContainsKey("_t"))
                {
                    Material["_t"] = "BH.oM.LadybugTools.IEnergyMaterialOpaque";
                    newVersion["ShadeMaterial"] = Material;
                }
            }

            return newVersion;
        }


        //already List<double>, but what about List<double?>?
        private static Dictionary<string, object> UpgradeTypology(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            List<double> numbers;

            return newVersion;
        }

        //So much stuff to version!
        private static Dictionary<string, object> UpgradeExternalComfort(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            Dictionary<string, object> UTC;

            if (newVersion.ContainsKey("UniversalThermalClimate"))
            {
                (newVersion["UniversalThermalClimate"] as Dictionary<string, object>)["_t"] = "BH.oM.LadybugTools.HourlyContinuousCollection";
            }
            if (newVersion.ContainsKey("DryBulbTemperature"))
            {
                (newVersion["DryBulbTemperature"] as Dictionary<string, object>)["_t"] = "BH.oM.LadybugTools.HourlyContinuousCollection";
            }
            if (newVersion.ContainsKey("RelativeHumidity"))
            {
                (newVersion["RelativeHumidity"] as Dictionary<string, object>)["_t"] = "BH.oM.LadybugTools.HourlyContinuousCollection";
            }
            if (newVersion.ContainsKey("WindSpeed"))
            {
                (newVersion["WindSpeed"] as Dictionary<string, object>)["_t"] = "BH.oM.LadybugTools.HourlyContinuousCollection";
            }
            if (newVersion.ContainsKey("MeanRadiantTemperature"))
            {
                (newVersion["MeanRadiantTemperature"] as Dictionary<string, object>)["_t"] = "BH.oM.LadybugTools.HourlyContinuousCollection";
            }

            /*if (oldVersion.TryGetValue("UniversalThermalClimate", out object obj))
            {
                UTC = obj as Dictionary<string, object>;
                if (UTC.ContainsKey("_t"))
                {
                    UTC["_t"] = "BH.oM.LadybugTools.HourlyContinuousCollection";
                }
                newVersion["UniversalThermalClimate"] = UTC;
            }*/

            return newVersion;
        }
    }
}
