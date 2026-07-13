/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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
    [Upgrader(9, 3)]
    public static class v93
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [VersioningTarget("BH.Revit.oM.Tagging.Settings.MEPGlobalTagSettings")]
        public static Dictionary<string, object> UpgradeMEPGlobalTagSettings(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            object bhomSettingsObject;
            if (newVersion.TryGetValue("BhomSettings", out bhomSettingsObject))
            {
                Dictionary<string, object> bhomSettings = bhomSettingsObject as Dictionary<string, object>;
                object useExclusionZones;
                if (bhomSettings != null && bhomSettings.TryGetValue("UseExclusionZones", out useExclusionZones))
                {
                    newVersion["UseExclusionZones"] = useExclusionZones;
                    bhomSettings.Remove("UseExclusionZones");
                }
            }

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.Tagging.Settings.PointTagSettings")]
        [VersioningTarget("BH.oM.Tagging.Settings.CurveTagSettings")]
        [VersioningTarget("BH.oM.Tagging.Settings.AreaTagSettings")]
        public static Dictionary<string, object> UpgradeBaseTagSettings(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            object baseSettingsObject;
            if (newVersion.TryGetValue("BaseSettings", out baseSettingsObject))
            {
                Dictionary<string, object> baseSettings = baseSettingsObject as Dictionary<string, object>;
                if (baseSettings != null)
                {
                    foreach (KeyValuePair<string, object> kvp in baseSettings)
                    {
                        if (kvp.Key == "_t")
                            continue;

                        newVersion[kvp.Key] = kvp.Value;
                    }
                }

                newVersion.Remove("BaseSettings");
            }

            return newVersion;
        }

        /***************************************************/
    }
}