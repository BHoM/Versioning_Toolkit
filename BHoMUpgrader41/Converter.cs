/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

namespace BH.Upgrader.v41
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "4.0";

            ToNewObject.Add("BH.oM.Programming.DataParam", UpdateNodeParam);
            ToNewObject.Add("BH.oM.Programming.ReceiverParam", UpdateNodeParam);
            ToNewObject.Add("BH.oM.CFD.Harpoon.HarpoonSettings", UpgradeHarpoonSettings);
            ToNewObject.Add("BH.oM.CFD.CFX.RadiantSource", UpgradeRadiantSource);
            ToNewObject.Add("BH.oM.CFD.CFX.AdiabaticBoundary", UpgradeAdiabaticBoundary);
            ToNewObject.Add("BH.oM.CFD.CFX.HeatFluxBoundary", UpgradeHeatFluxBoundary);
            ToNewObject.Add("BH.oM.CFD.CFX.HeatTransferBoundary", UpgradeHeatTransferBoundary);
            ToNewObject.Add("BH.oM.CFD.CFX.TemperatureBoundary", UpgradeTemperatureBoundary);
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, object> UpdateNodeParam(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            newVersion.Remove("ParentId");

            return newVersion;
        }

        public static Dictionary<string, object> UpgradeHarpoonSettings(Dictionary<string, object> harpoonSettings)
        {
            if (harpoonSettings == null)
                return null;

            Dictionary<string, object> newHarpoonSettings = new Dictionary<string, object>(harpoonSettings);
            if (!newHarpoonSettings.ContainsKey("AdvanceConfig"))
            {
                newHarpoonSettings.Add("AdvanceConfig", new List<string>());
            }

            return newHarpoonSettings;
        }

        public static Dictionary<string, object> UpgradeRadiantSource(Dictionary<string, object> radiantSource)
        {
            if (radiantSource == null)
                return null;

            Dictionary<string, object> energySource = new Dictionary<string, object>(radiantSource);
            energySource["_t"] = energySource["_t"].ToString().Replace("RadiantSource", "EnergySource");

            if (!energySource.ContainsKey("EnergyFluxInput"))
            {
                energySource.Add("EnergyFluxInput", "0 [W m^-2]");
            }
            if (energySource.ContainsKey("Direction"))
            {
                energySource.Add("RadiationDirection", radiantSource["Direction"]);
                energySource.Remove("Direction");
            }

            return energySource;
        }

        public static Dictionary<string, object> UpgradeAdiabaticBoundary(Dictionary<string, object> adiabaticBoundary)
        {
            if (adiabaticBoundary == null)
                return null;

            Dictionary<string, object> newAdiabaticBoundary = new Dictionary<string, object>(adiabaticBoundary);
            if (newAdiabaticBoundary.ContainsKey("RadiantSource"))
            {
                newAdiabaticBoundary.Add("EnergySource", newAdiabaticBoundary["RadiantSource"]);
                newAdiabaticBoundary.Remove("RadiantSource");
            }

            return newAdiabaticBoundary;
        }

        public static Dictionary<string, object> UpgradeHeatFluxBoundary(Dictionary<string, object> heatFluxBoundary)
        {
            if (heatFluxBoundary == null)
                return null;

            Dictionary<string, object> newHeatFluxBoundary = new Dictionary<string, object>(heatFluxBoundary);
            if (newHeatFluxBoundary.ContainsKey("RadiantSource"))
            {
                newHeatFluxBoundary.Add("EnergySource", newHeatFluxBoundary["RadiantSource"]);
                newHeatFluxBoundary.Remove("RadiantSource");
            }

            return newHeatFluxBoundary;
        }

        public static Dictionary<string, object> UpgradeHeatTransferBoundary(Dictionary<string, object> heatTransferBoundary)
        {
            if (heatTransferBoundary == null)
                return null;

            Dictionary<string, object> newHeatTransferBoundary = new Dictionary<string, object>(heatTransferBoundary);
            if (newHeatTransferBoundary.ContainsKey("RadiantSource"))
            {
                newHeatTransferBoundary.Add("EnergySource", newHeatTransferBoundary["RadiantSource"]);
                newHeatTransferBoundary.Remove("RadiantSource");
            }

            return newHeatTransferBoundary;
        }

        public static Dictionary<string, object> UpgradeTemperatureBoundary(Dictionary<string, object> temperatureBoundary)
        {
            if (temperatureBoundary == null)
                return null;

            Dictionary<string, object> newTemperatureBoundary = new Dictionary<string, object>(temperatureBoundary);
            if (newTemperatureBoundary.ContainsKey("RadiantSource"))
            {
                newTemperatureBoundary.Add("EnergySource", newTemperatureBoundary["RadiantSource"]);
                newTemperatureBoundary.Remove("RadiantSource");
            }

            return newTemperatureBoundary;
        }
        /***************************************************/
    }
}

