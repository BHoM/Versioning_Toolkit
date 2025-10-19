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

using BH.oM.Versioning;
using System.Collections.Generic;
using System;

namespace BH.Upgraders
{
    [Upgrader(9, 0)]
    public static class v90
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [VersioningTarget("BH.oM.BHoMAnalytics.ToolkitSettings")]
        [VersioningTarget("BH.oM.Analytics.ToolkitSettings")]
        public static Dictionary<string, object> UpgradeToolkitSettings(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.oM.BHoMAnalytics.ToolkitSettings";
            newVersion["Name"] = oldVersion["Name"];
            newVersion["ServerAddress"] = oldVersion["ServerAddress"];
            newVersion["DatabaseName"] = oldVersion["DatabaseName"];
            newVersion["CollectionName"] = oldVersion["CollectionName"];
            newVersion["InitialisationMethod"] = oldVersion["InitialisationMethod"];

            switch(oldVersion["InitialisationMethod"].ToString())
            {
                case "BH.Adapter.BHoMAnalytics.BHoMAnalyticsAdapter.InitialiseAnalytics":
                    newVersion["InitialisationAssembly"] = "BHoMAnalytics_Adapter";
                    break;
                case "BH.Engine.Analytics.Initialise.InitialiseAnalytics":
                    newVersion["InitialisationAssembly"] = "BuroHappold_BHoMAnalytics_Engine";
                    break;
                default:
                    newVersion["InitialisationAssembly"] = null;
                    break;
            }

            return newVersion;
        }

        /***************************************************/
    }
}
