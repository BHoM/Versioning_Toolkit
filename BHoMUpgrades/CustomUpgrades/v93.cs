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
using System;
using System.Collections.Generic;

namespace BH.Upgraders
{
    [Upgrader(9, 3)]
    public static class v93
    {
        /***************************************************/
        /****              Public Methods               ****/
        /***************************************************/

        [VersioningTarget("BH.Revit.oM.Tagging.Settings.MEPGlobalTagSettings")]
        public static Dictionary<string, object> UpgradeMEPGlobalTagSettings(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.TryGetValue("BhomSettings", out object bhomSettingsObject))
            {
                Dictionary<string, object> bhomSettings = bhomSettingsObject as Dictionary<string, object>;

                if (bhomSettings != null && bhomSettings.TryGetValue("UseExclusionZones", out object useExclusionZones))
                {
                    newVersion["UseExclusionZones"] = useExclusionZones;
                    bhomSettings.Remove("UseExclusionZones");
                }
            }

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.Tagging.Settings.PointTagSettings")]
        public static Dictionary<string, object> UpgradePointTagSettings(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = FlattenBaseSettings(oldVersion);
            if (newVersion == null)
                return null;

            //RiserLeaderAngle renamed to OrthogonalLeaderAngle on PointTagSettings.
            if (newVersion.TryGetValue("RiserLeaderAngle", out object riserLeaderAngle))
            {
                newVersion["OrthogonalLeaderAngle"] = riserLeaderAngle;
                newVersion.Remove("RiserLeaderAngle");
            }

            //TagObscuredDisciplineElements moved out of BaseTagSettings entirely, onto the Revit-side BaseRevitTagSettings
            newVersion.Remove("TagObscuredDisciplineElements");

            return newVersion;
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

            //See UpgradePointTagSettings for why TagObscuredDisciplineElements is dropped here too.
            newVersion.Remove("TagObscuredDisciplineElements");

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

            //See UpgradePointTagSettings for why TagObscuredDisciplineElements is dropped here too.
            newVersion.Remove("TagObscuredDisciplineElements");

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.Revit.oM.ElementRelationships.PlacementSettings")]
        public static Dictionary<string, object> UpgradePlacementSettings(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("PlacementCollections"))
            {
                newVersion.Remove("ElementPlacementSettings");
                return newVersion;
            }

            if (!newVersion.TryGetValue("ElementPlacementSettings", out object oldPlacementsObject))
                return newVersion;

            List<object> placements = new List<object>();
            IEnumerable<object> oldPlacements = oldPlacementsObject as IEnumerable<object>;
            if (oldPlacements != null)
            {
                foreach (object item in oldPlacements)
                {
                    if (item is Dictionary<string, object> placementDict)
                    {
                        if (placementDict.TryGetValue("_t", out object type) &&
                            type?.ToString() == "BH.Revit.oM.ElementRelationships.ElementPlacementSettings")
                        {
                            placementDict["_t"] = "BH.Revit.oM.ElementRelationships.ElementPlacement";
                        }

                        placements.Add(placementDict);
                    }
                    else
                    {
                        placements.Add(item);
                    }
                }
            }

            Dictionary<string, object> defaultCollection = new Dictionary<string, object>
            {
                { "_t", "BH.Revit.oM.ElementRelationships.PlacementCollection" },
                { "BHoM_Guid", Guid.NewGuid() },
                { "Name", "Default Collection" },
                { "Placements", placements }
            };

            newVersion["PlacementCollections"] = new List<object> { defaultCollection };
            newVersion.Remove("ElementPlacementSettings");

            return newVersion;
        }

        /***************************************************/
        /****             Private Methods               ****/
        /***************************************************/

        private static Dictionary<string, object> FlattenBaseSettings(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.TryGetValue("BaseSettings", out object baseSettingsObject))
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