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
using System.Xml.Linq;

namespace BH.Upgrader.v81
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "8.0";
            ToNewObject.Add("BH.oM.LadybugTools.SolarPanelTiltOptimisationCommand", UpgradeSolarTiltCommand);
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, object> UpgradeSolarTiltCommand(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            if (oldVersion.ContainsKey("_t"))
                newVersion["_t"] = "BH.oM.LadybugTools.SolarRadiationPlotCommand";

            if (oldVersion.ContainsKey("Azimuths"))
            {
                newVersion.TryGetValue("Azimuths", out object azimuths);
                newVersion.Remove("Azimuths");
                newVersion.Add("Directions", (int)azimuths);
            }

            if (oldVersion.ContainsKey("Altitudes"))
            {
                newVersion.TryGetValue("Altitudes", out object altitudes);
                newVersion.Remove("Altitudes");
                newVersion.Add("Tilts", (int)altitudes);
            }

            if (oldVersion.ContainsKey("GroundReflectance"))
                newVersion.Remove("GroundReflectance");

            if (oldVersion.ContainsKey("Isotropic"))
                newVersion.Remove("Isotropic");
            
            return newVersion;
        }
    }
}

