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

namespace BH.Upgraders
{
    [Upgrader(10, 0)]
    public static class v100
    {
        /***************************************************/
        /****              Public Methods               ****/
        /***************************************************/

        [VersioningTarget("BH.Revit.oM.Schematic.EquipmentAssemblyItem")]
        public static Dictionary<string, object> UpgradeEquipmentAssemblyItem(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            if (!newVersion.TryGetValue("RevitCategoryId", out object rawCategory))
                return newVersion;

            newVersion.Remove("RevitCategoryId");
            if (!TryGetInt64(rawCategory, out long categoryId) || categoryId == 0)
                return newVersion;

            Dictionary<string, object> filter = null;
            if (newVersion.TryGetValue("ElementsFilter", out object filterObject))
                filter = filterObject as Dictionary<string, object>;

            if (filter == null)
            {
                newVersion["ElementsFilter"] = new Dictionary<string, object>
                {
                    { "_t", "BH.Revit.oM.UI.Filtering.ElementFilterSelection" },
                    { "DocumentSelection", new Dictionary<string, object> { { "_t", "BH.Revit.oM.UI.Filtering.AllDocumentsSelection" } } },
                    { "CategoryIds", new List<long> { categoryId } },
                    { "ParameterFilters", EmptyParameterFilter() },
                    { "IsParameterFilteringEnabled", false }
                };
                return newVersion;
            }

            if (!filter.TryGetValue("CategoryIds", out object categories) || !HasItems(categories))
                filter["CategoryIds"] = new List<long> { categoryId };

            if (!filter.ContainsKey("ParameterFilters") || filter["ParameterFilters"] == null)
                filter["ParameterFilters"] = EmptyParameterFilter();

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.Revit.oM.UI.Filtering.ElementFilterSelection")]
        public static Dictionary<string, object> UpgradeElementFilterSelection(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            if (!newVersion.TryGetValue("DocumentIds", out object documentIds))
                return newVersion;

            newVersion.Remove("DocumentIds");
            if (!newVersion.ContainsKey("DocumentSelection"))
                newVersion["DocumentSelection"] = ToDocumentSelection(documentIds);

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.Revit.oM.Schematic.ParameterMapItem")]
        public static Dictionary<string, object> UpgradeParameterMapItem(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            UpgradeParameterSource(newVersion, "ParameterIn3D");
            UpgradeParameterSource(newVersion, "ParameterIn2D");
            return newVersion;
        }

        /***************************************************/
        /****              Private Methods              ****/
        /***************************************************/

        private static void UpgradeParameterSource(Dictionary<string, object> item, string property)
        {
            if (!item.TryGetValue(property, out object value) || !(value is string name))
                return;

            if (string.IsNullOrWhiteSpace(name))
            {
                item.Remove(property);
                return;
            }

            item[property] = new Dictionary<string, object>
            {
                { "_t", "BH.oM.Adapters.Revit.Parameters.ParameterValueSource" },
                { "ParameterName", name }
            };
        }

        /***************************************************/

        private static Dictionary<string, object> ToDocumentSelection(object documentIds)
        {
            List<long> ids = new List<long>();
            if (documentIds is IEnumerable items && !(documentIds is string))
            {
                foreach (object item in items)
                {
                    if (TryGetInt64(item, out long id))
                        ids.Add(id);
                }
            }

            if (ids.Count == 1 && ids[0] == -1)
                return new Dictionary<string, object> { { "_t", "BH.Revit.oM.UI.Filtering.HostDocumentSelection" } };

            bool host = false;
            List<long> links = new List<long>();
            foreach (long id in ids)
            {
                if (id == -1)
                    host = true;
                else
                    links.Add(id);
            }
            if (!host && links.Count > 0)
            {
                return new Dictionary<string, object>
                {
                    { "_t", "BH.Revit.oM.UI.Filtering.LinkDocumentSelection" },
                    { "Links", links }
                };
            }

            return new Dictionary<string, object>
            {
                { "_t", "BH.Revit.oM.UI.Filtering.HostAndLinkDocumentSelection" },
                { "Host", host },
                { "Links", links }
            };
        }

        /***************************************************/

        private static Dictionary<string, object> EmptyParameterFilter()
        {
            return new Dictionary<string, object>
            {
                { "_t", "BH.oM.Verification.Conditions.LogicalAndCondition" },
                { "Conditions", new List<object>() }
            };
        }

        /***************************************************/

        private static bool TryGetInt64(object value, out long number)
        {
            switch (value)
            {
                case int intValue:
                    number = intValue;
                    return true;
                case long longValue:
                    number = longValue;
                    return true;
                case double doubleValue:
                    number = (long)doubleValue;
                    return true;
                case string text when long.TryParse(text, out long parsed):
                    number = parsed;
                    return true;
                default:
                    try
                    {
                        number = Convert.ToInt64(value);
                        return true;
                    }
                    catch
                    {
                        number = 0;
                        return false;
                    }
            }
        }

        /***************************************************/

        private static bool HasItems(object value)
        {
            if (value is string || !(value is IEnumerable items))
                return false;

            foreach (object item in items)
            {
                if (item != null)
                    return true;
            }

            return false;
        }

        /***************************************************/
    }
}
