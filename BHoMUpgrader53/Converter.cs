/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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

namespace BH.Upgrader.v53
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "5.2";

            var revitTypeNames = new List<string>
            {
                "BH.oM.Revit.AddinManagementSettings",
                "BH.oM.Revit.Attributes.ComboBoxAttribute",
                "BH.oM.Revit.Attributes.IconFileNameAttribute",
                "BH.oM.Revit.Attributes.ManualTransactionAttribute",
                "BH.oM.Revit.Attributes.NameAttribute",
                "BH.oM.Revit.Attributes.PanelNameAttribute",
                "BH.oM.Revit.Attributes.SplitButtonAttribute",
                "BH.oM.Revit.Attributes.TabNameAttribute",
                "BH.oM.Revit.Attributes.TagsAttribute",
                "BH.oM.Revit.Attributes.ToolTipAttribute",
                "BH.oM.Revit.Attributes.ToolTipImageFileNameAttribute",
                "BH.oM.Revit.Attributes.UIEnabledAttribute",
                "BH.oM.Revit.DockablePaneSettings",
                "BH.oM.Revit.IApplicationServiceSettings",
                "BH.oM.Revit.IRevitSettings",
                "BH.oM.Revit.Logging.BaseRevitLogItem",
                "BH.oM.Revit.Logging.IRevitLogItem",
                "BH.oM.Revit.PinnedItemInfo",
                "BH.oM.Revit.Scenarios.ElementFilter",
                "BH.oM.Revit.Scenarios.IScenario",
                "BH.oM.Revit.Scenarios.IScenario1Element",
                "BH.oM.Revit.Scenarios.IScenario2Elements",
                "BH.oM.Revit.Scenarios.IScenario3Elements",
                "BH.oM.Revit.UIManagementSettings",
            };
            foreach (string name in revitTypeNames)
            {
                ToNewObject.Add(name, CloneVersion);
            }
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        public static Dictionary<string, object> CloneVersion(Dictionary<string, object> oldVersion)
        {
            return new Dictionary<string, object>(oldVersion);
        }

        /***************************************************/
    }
}
