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

        [VersioningTarget("BH.Revit.oM.Tagging.Settings.RevitPointTagSettings")]
        [VersioningTarget("BH.Revit.oM.Tagging.Settings.RevitCurveTagSettings")]
        [VersioningTarget("BH.Revit.oM.Tagging.Settings.RevitAreaTagSettings")]
        [VersioningTarget("BH.Revit.oM.Tagging.Settings.MEPPointTagSettings")]
        [VersioningTarget("BH.Revit.oM.Tagging.Settings.MEPCurveTagSettings")]
        [VersioningTarget("BH.Revit.oM.Tagging.Settings.MEPAreaTagSettings")]
        [VersioningTarget("BH.Revit.oM.Tagging.Settings.StructurePointTagSettings")]
        [VersioningTarget("BH.Revit.oM.Tagging.Settings.StructureCurveTagSettings")]
        [VersioningTarget("BH.Revit.oM.Tagging.Settings.StructureAreaTagSettings")]
        public static Dictionary<string, object> UpgradeRevitTagSettingsTagObscured(Dictionary<string, object> oldVersion)
        {
            //TagObscuredDisciplineElements moved from the nested BhomSettings (BH.oM.Tagging.Settings.BaseTagSettings)
            //up to this Revit-side settings object (BH.Revit.oM.Tagging.Settings.BaseRevitTagSettings), since it is
            //only ever consumed by Revit-specific tagging logic.
            return HoistFromBhomSettings(oldVersion, "TagObscuredDisciplineElements");
        }

        /***************************************************/

        [VersioningTarget("BH.oM.Tagging.Settings.PointTagSettings")]
        public static Dictionary<string, object> UpgradePointTagSettings(Dictionary<string, object> oldVersion)
        {
            return FlattenBaseSettings(oldVersion);
        }

        /***************************************************/

        [VersioningTarget("BH.oM.Tagging.Settings.CurveTagSettings")]
        public static Dictionary<string, object> UpgradeCurveTagSettings(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = FlattenBaseSettings(oldVersion);
            if (newVersion == null)
                return null;

            //KeepTagPlacementPointOnHost moved from BaseTagSettings to PointTagSettings and AreaTagSettings only.
            //It was never used by CurveTagSettings, so it is dropped rather than carried over.
            newVersion.Remove("KeepTagPlacementPointOnHost");

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.Tagging.Settings.AreaTagSettings")]
        public static Dictionary<string, object> UpgradeAreaTagSettings(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = FlattenBaseSettings(oldVersion);
            if (newVersion == null)
                return null;

            //RiserLeaderAngle moved from BaseTagSettings to PointTagSettings and CurveTagSettings only.
            //It was never used by AreaTagSettings, so it is dropped rather than carried over.
            newVersion.Remove("RiserLeaderAngle");

            return newVersion;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, object> HoistFromBhomSettings(Dictionary<string, object> oldVersion, string propertyName)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            object bhomSettingsObject;
            if (newVersion.TryGetValue("BhomSettings", out bhomSettingsObject))
            {
                Dictionary<string, object> bhomSettings = bhomSettingsObject as Dictionary<string, object>;
                object propertyValue;
                if (bhomSettings != null && bhomSettings.TryGetValue(propertyName, out propertyValue))
                {
                    newVersion[propertyName] = propertyValue;
                    bhomSettings.Remove(propertyName);
                }
            }

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> FlattenBaseSettings(Dictionary<string, object> oldVersion)
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