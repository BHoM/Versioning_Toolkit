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
using System;
using System.Collections.Generic;

namespace BH.Upgraders
{
    [Upgrader(8, 2)]
    public static class v82
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [VersioningTarget("BH.oM.Security.Elements.CameraDevice")]
        public static Dictionary<string, object> UpgradeCameraDevice(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.oM.Security.Elements.CameraDevice";
            newVersion["EyePosition"] = oldVersion["EyePosition"];
            newVersion["TargetPosition"] = oldVersion["TargetPosition"];
            newVersion["Mounting"] = oldVersion["Mounting"];
            newVersion["Type"] = oldVersion["Type"];
            newVersion["Megapixels"] = oldVersion["Megapixels"];

            if (oldVersion.ContainsKey("HorizontalFieldOfView"))
            {
                double hfov = (double)oldVersion["HorizontalFieldOfView"];
                double distance = double.NaN;

                if (oldVersion.ContainsKey("EyePosition") && oldVersion.ContainsKey("TargetPosition"))
                {
                    Dictionary<string, object> ptA = oldVersion["EyePosition"] as Dictionary<string, object>;
                    Dictionary<string, object> ptB = oldVersion["TargetPosition"] as Dictionary<string, object>;
                    if (ptA != null && ptB != null)
                        distance = DistanceBetweenPoints(ptA, ptB);
                }

                double angle = 2 * Math.Atan(hfov / 2 / distance);
                newVersion["Angle"] = angle;
            }

            return newVersion;
        }

        /***************************************************/

        private static double DistanceBetweenPoints(Dictionary<string, object> pt1, Dictionary<string, object> pt2)
        {
            double x1 = Convert.ToDouble(pt1["X"]);
            double y1 = Convert.ToDouble(pt1["Y"]);
            double z1 = Convert.ToDouble(pt1["Z"]);
            double x2 = Convert.ToDouble(pt2["X"]);
            double y2 = Convert.ToDouble(pt2["Y"]);
            double z2 = Convert.ToDouble(pt2["Z"]);

            return Math.Sqrt(
                Math.Pow(x2 - x1, 2) +
                Math.Pow(y2 - y1, 2) +
                Math.Pow(z2 - z1, 2)
            );
        }

        /***************************************************/

        [VersioningTarget("BH.oM.Adapters.Revit.Parameters.RevitParameter")]
        public static Dictionary<string, object> UpgradeRevitParameter(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            if (newVersion.ContainsKey("UnitType"))
            {
                newVersion["Quantity"] = newVersion["UnitType"];
                newVersion.Remove("UnitType");
            }
            else
                newVersion["Quantity"] = "";

            newVersion["Unit"] = "";

            if (!newVersion.ContainsKey("Name"))
                newVersion["Name"] = "";

            if (!newVersion.ContainsKey("Value"))
                newVersion["Value"] = null;

            if (!newVersion.ContainsKey("IsReadOnly"))
                newVersion["IsReadOnly"] = false;

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.WaterDeprivationMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.PhotochemicalOzoneCreationTRACIMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.PhotochemicalOzoneCreationMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.PhotochemicalOzoneCreationCMLMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.OzoneDepletionMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.EutrophicationTRACIMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.EutrophicationTerrestrialMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.EutrophicationCMLMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.EutrophicationAquaticMarineMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.EutrophicationAquaticFreshwaterMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.ClimateChangeTotalNoBiogenicMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.ClimateChangeTotalMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.ClimateChangeLandUseMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.ClimateChangeFossilMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.ClimateChangeBiogenicMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.AcidificationMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.AbioticDepletionMineralsAndMetalsMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.MaterialFragments.AbioticDepletionFossilResourcesMetric")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.WaterDeprivationMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.PhotochemicalOzoneCreationTRACIMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.PhotochemicalOzoneCreationMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.PhotochemicalOzoneCreationCMLMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.OzoneDepletionMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.EutrophicationTRACIMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.EutrophicationTerrestrialMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.EutrophicationCMLMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.EutrophicationAquaticMarineMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.EutrophicationAquaticFreshwaterMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.ClimateChangeTotalNoBiogenicMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.ClimateChangeTotalMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.ClimateChangeLandUseMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.ClimateChangeFossilMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.ClimateChangeBiogenicMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.AcidificationMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.AbioticDepletionMineralsAndMetalsMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.AbioticDepletionFossilResourcesMaterialResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.WaterDeprivationElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.PhotochemicalOzoneCreationTRACIElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.PhotochemicalOzoneCreationElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.PhotochemicalOzoneCreationCMLElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.OzoneDepletionElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.EutrophicationTRACIElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.EutrophicationTerrestrialElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.EutrophicationCMLElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.EutrophicationAquaticMarineElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.EutrophicationAquaticFreshwaterElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.ClimateChangeTotalNoBiogenicElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.ClimateChangeTotalElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.ClimateChangeLandUseElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.ClimateChangeFossilElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.ClimateChangeBiogenicElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.AcidificationElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.AbioticDepletionMineralsAndMetalsElementResult")]
        [VersioningTarget("BH.oM.LifeCycleAssessment.Results.AbioticDepletionFossilResourcesElementResult")]
        public static Dictionary<string, object> UpgradeMetricAndResult(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            if(newVersion.ContainsKey("MetricType"))
                newVersion.Remove("MetricType");
			
            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.Verification.Results.RequirementResult")]
        public static Dictionary<string, object> UpgradeRequirementResult(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            newVersion["Events"] = null;

            return newVersion;
        }

        /***************************************************/
    }
}
