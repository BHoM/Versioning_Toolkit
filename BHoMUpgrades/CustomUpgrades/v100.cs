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

namespace BH.Upgraders
{
    [Upgrader(10, 0)]
    public static class v100
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [VersioningTarget("BH.Revit.oM.UI.UIManagementSettings")]
        public static Dictionary<string, object> UpgradeUIManagementSettings(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.Revit.oM.UI.ButtonLayoutSettings";

            if (oldVersion.ContainsKey("Name"))
                newVersion["Name"] = oldVersion["Name"];

            if (oldVersion.ContainsKey("BHoM_Guid"))
                newVersion["BHoM_Guid"] = oldVersion["BHoM_Guid"];

            if (oldVersion.ContainsKey("CustomData"))
                newVersion["CustomData"] = oldVersion["CustomData"];

            Dictionary<string, object> buttonLayout = new Dictionary<string, object>();
            buttonLayout["_t"] = "BH.Revit.oM.UI.ButtonLayout";
            buttonLayout["Tabs"] = new List<Dictionary<string, object>>();

            if (oldVersion.ContainsKey("PinnedItems"))
            {
                List<Dictionary<string, object>> pinnedItems = oldVersion["PinnedItems"] as List<Dictionary<string, object>>;
                if (pinnedItems != null)
                {
                    Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>> tabPanelMapping = new Dictionary<string, Dictionary<string, List<Dictionary<string, object>>>>();
                    foreach (Dictionary<string, object> item in pinnedItems)
                    {
                        if (!item.ContainsKey("_t"))
                            continue;

                        if (item["_t"] != "BH.Revit.oM.UI.PinnedItemInfo")
                            continue;

                        if (!item.ContainsKey("PanelName") || !item.ContainsKey("TabName"))
                            continue;

                        string tabName = item["TabName"] as string;
                        string panelName = item["PanelName"] as string;

                        if (string.IsNullOrWhiteSpace(tabName) || string.IsNullOrWhiteSpace(panelName))
                            continue;

                        if (!tabPanelMapping.ContainsKey(tabName))
                            tabPanelMapping[tabName] = new Dictionary<string, List<Dictionary<string, object>>>();

                        if (!tabPanelMapping[tabName].ContainsKey(panelName))
                            tabPanelMapping[tabName][panelName] = new List<Dictionary<string, object>>();

                        tabPanelMapping[tabName][panelName].Add(item);
                    }

                    foreach (KeyValuePair<string, Dictionary<string, List<Dictionary<string, object>>>> tab in tabPanelMapping)
                    {
                        Dictionary<string, object> tabDict = new Dictionary<string, object>();
                        tabDict["_t"] = "BH.Revit.oM.UI.TabLayout";
                        tabDict["Name"] = tab.Key;
                        tabDict["Panels"] = new List<Dictionary<string, object>>();
                        foreach (KeyValuePair<string, List<Dictionary<string, object>>> panel in tab.Value)
                        {
                            Dictionary<string, object> panelDict = new Dictionary<string, object>();
                            panelDict["_t"] = "BH.Revit.oM.UI.Panel";
                            panelDict["Name"] = panel.Key;
                            panelDict["Items"] = panel.Value;
                            (tabDict["Panels"] as List<Dictionary<string, object>>).Add(panelDict);
                        }
                        (buttonLayout["Tabs"] as List<Dictionary<string, object>>).Add(tabDict);
                    }
                }
            }

            newVersion["Layout"] = buttonLayout;

            return newVersion;
        }

        /***************************************************/
    }
}