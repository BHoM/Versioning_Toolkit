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
using System.ComponentModel;
using System.Linq;
using System.Text;
using BH.Upgrader.Base;
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
            ToNewObject.Add("BH.Revit.oM.UI.Filters.ParameterFilterRule", UpgradeParameterFilterRule);
            ToNewObject.Add("BH.Revit.oM.UI.Filters.ParameterFilterSet", UpgradeParameterFilterSet);
            ToNewObject.Add("BH.Revit.oM.UI.Filters.ParameterItem", UpgradeParameterItem);
            ToNewObject.Add("BH.Revit.oM.UI.SelectFromListViewSettings", UpgradeSelectFromListViewSettings);
            ToNewObject.Add("BH.Revit.oM.ModelQA.Extraction.BaseExtractFromDocument", UpgradeBaseExtractFromDocument);
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

        /***************************************************/

        private static Dictionary<string, object> UpgradeParameterFilterRule(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.oM.Verification.Conditions.ValueCondition";

            if (oldVersion.ContainsKey("ParameterValue"))
                newVersion["ReferenceValue"] = oldVersion["ParameterValue"];

            if (oldVersion.ContainsKey("FilteringParameter"))
                newVersion["ValueSource"] = oldVersion["FilteringParameter"];

            var compType = ValueCompType((string)oldVersion["FilterCondition"]);
            newVersion["ComparisonType"] = compType.Item1;

            if (compType.Item2)
            {
                Dictionary<string, object> inverted = new Dictionary<string, object>();
                inverted["_t"] = "BH.oM.Verification.Conditions.LogicalNotCondition";
                inverted["Condition"] = newVersion;
                newVersion = inverted;
            }

            return newVersion;
        }

        /***************************************************/

        private static (string, bool) ValueCompType(string i)
        {
            switch (i)
            {
                case "Equal":
                    return ("EqualTo", false);
                case "NotEqual":
                    return ("NotEqualTo", false);
                case "Contains":
                    return ("Contains", false);
                case "NotContain":
                    return ("Contains", true);
                case "Greater":
                    return ("GreaterThan", false);
                case "GreaterOrEqual":
                    return ("GreaterThanOrEqualTo", false);
                case "Less":
                    return ("LessThan", false);
                case "LessOrEqual":
                    return ("LessThanOrEqualTo", false);
                default:
                    throw new NoUpdateException("Versioning failed due to an unknown enum value.");
            }
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeParameterFilterSet(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            if ((string)oldVersion["FilterCombination"] == "And")
                newVersion["_t"] = "BH.oM.Verification.Conditions.LogicalAndCondition";
            else
                newVersion["_t"] = "BH.oM.Verification.Conditions.LogicalOrCondition";

            List<Dictionary<string, object>> conditions = new List<Dictionary<string, object>>();

            if (oldVersion.ContainsKey("FilterRules"))
            {

                foreach (Dictionary<string, object> filterRule in (dynamic)oldVersion["FilterRules"])
                {
                    conditions.Add(filterRule);
                }
            }

            if (oldVersion.ContainsKey("FilterSets"))
            {

                foreach (Dictionary<string, object> filterSet in (dynamic)oldVersion["FilterSets"])
                {
                    conditions.Add(filterSet);
                }
            }

            newVersion["Conditions"] = conditions;
            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeParameterItem(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.oM.Adapters.Revit.Parameters.ParameterValueSource";

            if (oldVersion.ContainsKey("Name"))
                newVersion["ParameterName"] = oldVersion["Name"];

            if (oldVersion.ContainsKey("IsType"))
                newVersion["FromType"] = oldVersion["IsType"];

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeSelectFromListViewSettings(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.Revit.oM.UI.Filtering.ElementFilterSelection";

            if (oldVersion.ContainsKey("FilteringSettings"))
            {
                Dictionary<string, object> filteringSettings = oldVersion["FilteringSettings"] as Dictionary<string, object>;
                if (filteringSettings != null)
                {
                    if (filteringSettings.ContainsKey("Documents"))
                    {
                        List<int> documentSelection = new List<int>();
                        foreach (Dictionary<string, object> documentItem in (dynamic)filteringSettings["Documents"])
                        {
                            documentSelection.Add((int)documentItem["LinkInstanceId"]);
                        }

                        newVersion["DocumentIds"] = documentSelection;
                    }

                    if (filteringSettings.ContainsKey("Categories"))
                    {
                        List<int> categorySelection = new List<int>();
                        foreach (Dictionary<string, object> categoryItem in (dynamic)filteringSettings["Categories"])
                        {
                            categorySelection.Add((int)categoryItem["Id"]);
                        }

                        newVersion["CategoryIds"] = categorySelection;
                    }
                }
            }

            if (oldVersion.ContainsKey("ParameterFilteringSettings"))
            {
                Dictionary<string, object> paramFilteringSettings = oldVersion["ParameterFilteringSettings"] as Dictionary<string, object>;
                if (paramFilteringSettings != null && paramFilteringSettings.ContainsKey("FilterSet"))
                {
                    newVersion["ParameterFilters"] = paramFilteringSettings["FilterSet"];
                    newVersion["IsParameterFilteringEnabled"] = true;
                }
            }

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeBaseExtractFromDocument(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.Revit.oM.UI.Filtering.FilterBasedExtraction";

            List<Dictionary<string, object>> prefilters = new List<Dictionary<string, object>>();

            Dictionary<string, object> instanceVsTypeCondition = new Dictionary<string, object>();
            instanceVsTypeCondition["_t"] = "BH.Revit.oM.UI.Filtering.Conditions.InstanceVsTypeCondition";
            instanceVsTypeCondition["Instance"] = oldVersion["Instances"];
            instanceVsTypeCondition["Type"] = oldVersion["Types"];
            prefilters.Add(instanceVsTypeCondition);

            if (oldVersion.ContainsKey("CategoryNames"))
            {
                List<string> categoryNames = oldVersion["CategoryNames"] as List<string>;
                if (categoryNames != null)
                {
                    Dictionary<string, object> categoryCondition = new Dictionary<string, object>();
                    categoryCondition["_t"] = "BH.Revit.oM.UI.Filtering.Conditions.CategoryNameCondition";
                    categoryCondition["CategoryNames"] = new HashSet<string>(categoryNames);
                    prefilters.Add(categoryCondition);
                }
            }

            newVersion["Prefilters"] = prefilters;

            if (oldVersion.ContainsKey("FilterCondition"))
                newVersion["Filter"] = oldVersion["FilterCondition"];

            return newVersion;
        }

        /***************************************************/
    }
}
