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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgraders
{
    [Upgrader(5, 3)]
    public static class v53
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [VersioningTarget("BH.oM.Revit.AddinManagementSettings")]
        [VersioningTarget("BH.oM.Revit.Attributes.ComboBoxAttribute")]
        [VersioningTarget("BH.oM.Revit.Attributes.IconFileNameAttribute")]
        [VersioningTarget("BH.oM.Revit.Attributes.ManualTransactionAttribute")]
        [VersioningTarget("BH.oM.Revit.Attributes.NameAttribute")]
        [VersioningTarget("BH.oM.Revit.Attributes.PanelNameAttribute")]
        [VersioningTarget("BH.oM.Revit.Attributes.SplitButtonAttribute")]
        [VersioningTarget("BH.oM.Revit.Attributes.TabNameAttribute")]
        [VersioningTarget("BH.oM.Revit.Attributes.TagsAttribute")]
        [VersioningTarget("BH.oM.Revit.Attributes.ToolTipAttribute")]
        [VersioningTarget("BH.oM.Revit.Attributes.ToolTipImageFileNameAttribute")]
        [VersioningTarget("BH.oM.Revit.Attributes.UIEnabledAttribute")]
        [VersioningTarget("BH.oM.Revit.DockablePaneSettings")]
        [VersioningTarget("BH.oM.Revit.IApplicationServiceSettings")]
        [VersioningTarget("BH.oM.Revit.IRevitSettings")]
        [VersioningTarget("BH.oM.Revit.Logging.BaseRevitLogItem")]
        [VersioningTarget("BH.oM.Revit.Logging.IRevitLogItem")]
        [VersioningTarget("BH.oM.Revit.PinnedItemInfo")]
        [VersioningTarget("BH.oM.Revit.Scenarios.ElementFilter")]
        [VersioningTarget("BH.oM.Revit.Scenarios.IScenario")]
        [VersioningTarget("BH.oM.Revit.Scenarios.IScenario1Element")]
        [VersioningTarget("BH.oM.Revit.Scenarios.IScenario2Elements")]
        [VersioningTarget("BH.oM.Revit.Scenarios.IScenario3Elements")]
        [VersioningTarget("BH.oM.Revit.UIManagementSettings")]
        [VersioningTarget("BH.oM.Revit.Logging.StandardRevitLog")]
        [VersioningTarget("BH.oM.Revit.Logging.StandardRevitLog`1[[BH.oM.Revit.Logging.BaseRevitLogItem]]")]
        public static Dictionary<string, object> CloneVersion(Dictionary<string, object> oldVersion)
        {
            var newVersion = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> kvp in oldVersion)
            {
                string key = kvp.Key;
                object value = kvp.Value;
                if (key == "_t")
                {
                    value = value.ToString().Replace("BuroHappold_Revit_oM", "Revit_UI_oM");
                    value = value.ToString().Replace("BuroHappold_Revit_Engine", "Revit_UI_Engine");
                    value = value.ToString().Replace("BuroHappold_Revit_UI", "Revit_UI");
                }
                newVersion[key] = value;
            }

            return newVersion;
        }

        /***************************************************/
    }
}



