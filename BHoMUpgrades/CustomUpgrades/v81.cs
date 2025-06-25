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

using System.Collections.Generic;
using System.Linq;

using BH.oM.Versioning;

namespace BH.Upgraders
{
    [Upgrader(8, 1)]
    public static class v81
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [VersioningTarget("BH.oM.LadybugTools.SolarPanelTiltOptimisationCommand")]
        public static Dictionary<string, object> UpgradeSolarTiltCommand(Dictionary<string, object> oldVersion)
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

        [VersioningTarget("BH.Revit.oM.UI.Filters.ParameterFilterRule")]
        public static Dictionary<string, object> UpgradeParameterFilterRule(Dictionary<string, object> oldVersion)
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

        [VersioningTarget("BH.Revit.oM.UI.Filters.ParameterFilterSet")]
        public static Dictionary<string, object> UpgradeParameterFilterSet(Dictionary<string, object> oldVersion)
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

        [VersioningTarget("BH.Revit.oM.UI.Filters.ParameterItem")]
        public static Dictionary<string, object> UpgradeParameterItem(Dictionary<string, object> oldVersion)
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

        [VersioningTarget("BH.Revit.oM.UI.SelectFromListViewSettings")]
        public static Dictionary<string, object> UpgradeSelectFromListViewSettings(Dictionary<string, object> oldVersion)
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

        [VersioningTarget("BH.Revit.oM.ModelQA.Extraction.BaseExtractFromDocument")]
        public static Dictionary<string, object> UpgradeBaseExtractFromDocument(Dictionary<string, object> oldVersion)
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
                HashSet<string> categoryNames = new HashSet<string>();
                foreach (string categoryName in (dynamic)oldVersion["CategoryNames"])
                {
                    categoryNames.Add(categoryName);
                }

                if (categoryNames.Count != 0)
                {
                    Dictionary<string, object> categoryCondition = new Dictionary<string, object>();
                    categoryCondition["_t"] = "BH.Revit.oM.UI.Filtering.Conditions.CategoryNameCondition";
                    categoryCondition["CategoryNames"] = categoryNames;
                    prefilters.Add(categoryCondition);
                }
            }

            newVersion["Prefilters"] = prefilters;

            if (oldVersion.ContainsKey("FilterCondition"))
                newVersion["Filter"] = oldVersion["FilterCondition"];

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.Revit.oM.ElementRelationships.ElementParameter")]
        public static Dictionary<string, object> UpgradeElementParameter(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.oM.Adapters.Revit.Parameters.ParameterValueSource";
            newVersion["ParameterName"] = oldVersion["Name"];
            newVersion["FromType"] = oldVersion["FromElementType"];

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.LadybugTools.UTCIHeatPlotCommand")]
        public static Dictionary<string, object> UpgradeUTCIHeatPlotCommand(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.TryGetValue("GroundMaterial", out object gm) && newVersion.TryGetValue("ShadeMaterial", out object sm) && newVersion.TryGetValue("Typology", out object typ))
            {
                
                newVersion.TryGetValue("WindSpeedMultiplier", out object wsm);

                string epwFileName = (string)(oldVersion["EPWFile"] as Dictionary<string, object>)["Directory"] + "/"
                    + (string)(oldVersion["EPWFile"] as Dictionary<string, object>)["FileName"];

                Dictionary<string, object> externalComfort = new Dictionary<string, object>()
                {
                    { "_t", "BH.oM.LadybugTools.ExternalComfort" },
                    { "Typology", UpgradeTypology(typ as Dictionary<string, object>, wsm) },
                    { "SimulationResult", new Dictionary<string, object>()
                        {
                        { "_t", "BH.oM.LadybugTools.SimulationResult" },
                        { "EpwFile", epwFileName },
                        { "Name", "" },
                        { "GroundMaterial", gm },
                        { "ShadeMaterial", sm },
                        { "ShadedDownTemperature", null },
                        { "ShadedUpTemperature", null },
                        { "ShadedRadiantTemperature", null },
                        { "ShadedLongwaveMeanRadiantTemperatureDelta", null },
                        { "ShadedShortwaveMeanRadiantTemperatureDelta", null },
                        { "ShadedMeanRadiantTemperature", null },
                        { "UnshadedDownTemperature", null },
                        { "UnshadedUpTemperature", null },
                        { "UnshadedRadiantTemperature", null },
                        { "UnshadedLongwaveMeanRadiantTemperatureDelta", null },
                        { "UnshadedShortwaveMeanRadiantTemperatureDelta", null },
                        { "UnshadedMeanRadiantTemperature", null }
                        }
                    },
                    { "DryBulbTemperature", null },
                    { "RelativeHumidity", null },
                    { "WindSpeed", null },
                    { "MeanRadiantTemperature", null },
                    { "UniversalThermalClimateIndex", null }
                };
                newVersion.Remove("GroundMaterial");
                newVersion.Remove("ShadeMaterial");
                newVersion.Remove("Typology");

                newVersion.Add("ExternalComfort", externalComfort);
            }

            if (newVersion.ContainsKey("WindSpeedMultiplier"))
                newVersion.Remove("WindSpeedMultiplier");

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeTypology(Dictionary<string, object> oldVersion, object windSpeedMultiplier)
        {
            //this object has had a property added to it, which was loaned from UTCIHeatPlotCommand, which is why there is a cusom upgrader
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            if (!newVersion.ContainsKey("WindSpeedMultiplier"))
                newVersion.Add("WindSpeedMultiplier", windSpeedMultiplier);

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.LadybugTools.SimulationResult")]
        public static Dictionary<string, object> UpgradeSimulationResult(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("EpwFile"))
            {
                string epwFile = (string)oldVersion["EpwFile"];
                string fileName = epwFile.Split('\\', '/').LastOrDefault();
                string filePath = "";

                if (fileName == epwFile)
                {
                    filePath = fileName;
                    fileName = "";
                }

                if (!string.IsNullOrEmpty(fileName))
                    filePath = epwFile.Substring(0, epwFile.LastIndexOf(fileName) - 1);

                Dictionary<string, object> newEpwFile = new Dictionary<string, object>()
                {
                    { "_t", "BH.oM.Adapter.FileSettings" },
                    { "FileName", fileName },
                    { "Directory", filePath }
                };
                newVersion.Remove("EpwFile");
                newVersion.Add("EpwFile", newEpwFile);
            }

            return newVersion;
        }

        /***************************************************/
    }
}
