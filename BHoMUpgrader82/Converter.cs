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

namespace BH.Upgrader.v82
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "8.1";
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.AbioticDepletionFossilResourcesMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.AbioticDepletionMineralsAndMetalsMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.AcidificationMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.ClimateChangeBiogenicMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.ClimateChangeFossilMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.ClimateChangeLandUseMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.ClimateChangeTotalMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.ClimateChangeTotalNoBiogenicMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.EutrophicationAquaticFreshwaterMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.EutrophicationAquaticMarineMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.EutrophicationCMLMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.EutrophicationTerrestrialMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.EutrophicationTRACIMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.OzoneDepletionMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.PhotochemicalOzoneCreationCMLMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.PhotochemicalOzoneCreationMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.PhotochemicalOzoneCreationTRACIMetric", UpgradeMetric);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragments.WaterDeprivationMetric", UpgradeMetric);

            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.AbioticDepletionFossilResourcesMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.AbioticDepletionMineralsAndMetalsMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.AcidificationMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.ClimateChangeBiogenicMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.ClimateChangeFossilMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.ClimateChangeLandUseMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.ClimateChangeTotalMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.ClimateChangeTotalNoBiogenicMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.EutrophicationAquaticFreshwaterMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.EutrophicationAquaticMarineMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.EutrophicationCMLMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.EutrophicationTerrestrialMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.EutrophicationTRACIMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.OzoneDepletionMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.PhotochemicalOzoneCreationCMLMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.PhotochemicalOzoneCreationMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.PhotochemicalOzoneCreationTRACIMaterialResult", UpgradeMaterialResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.WaterDeprivationMaterialResult", UpgradeMaterialResult);

            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.AbioticDepletionFossilResourcesElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.AbioticDepletionMineralsAndMetalsElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.AcidificationElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.ClimateChangeBiogenicElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.ClimateChangeFossilElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.ClimateChangeLandUseElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.ClimateChangeTotalElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.ClimateChangeTotalNoBiogenicElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.EutrophicationAquaticFreshwaterElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.EutrophicationAquaticMarineElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.EutrophicationCMLElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.EutrophicationTerrestrialElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.EutrophicationTRACIElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.OzoneDepletionElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.PhotochemicalOzoneCreationCMLElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.PhotochemicalOzoneCreationElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.PhotochemicalOzoneCreationTRACIElementResult", UpgradeElementResult);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.Results.WaterDeprivationElementResult", UpgradeElementResult);

        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, object> UpgradeMetric(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();

            if(oldVersion.TryGetValue("_t", out object type))
                newVersion["_t"] = type;

            newVersion["Indicators"] = IndicatorsDics(oldVersion);
            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeMaterialResult(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();

            if (oldVersion.TryGetValue("_t", out object type))
                newVersion["_t"] = type;

            if (oldVersion.TryGetValue("MaterialName", out object matName))
                newVersion["MaterialName"] = matName;

            if (oldVersion.TryGetValue("EnvironmentalProductDeclarationName", out object epdName))
                newVersion["EnvironmentalProductDeclarationName"] = epdName;

            newVersion["Indicators"] = IndicatorsDics(oldVersion);
            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeElementResult(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();

            if (oldVersion.TryGetValue("_t", out object type))
                newVersion["_t"] = type;

            if (oldVersion.TryGetValue("ObjectId", out object objId))
                newVersion["ObjectId"] = objId;

            if (oldVersion.TryGetValue("Scope", out object scope))
                newVersion["Scope"] = scope;

            if (oldVersion.TryGetValue("Category", out object cat))
                newVersion["Category"] = cat;

            if (oldVersion.TryGetValue("MaterialResults", out object matResObj))
            {
                newVersion["MaterialResults"] = matResObj;  //Should already be upgraded by main loop upgrading subobjects
            }


            newVersion["Indicators"] = IndicatorsDics(oldVersion);
            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, double> IndicatorsDics(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, double> indicators = new Dictionary<string, double>();

            List<string> propertiesToCheck = new List<string>() {
                "A1",
                "A2",
                "A3",
                "A1toA3",
                "A4",
                "A5",
                "B1",
                "B2",
                "B3",
                "B4",
                "B5",
                "B6",
                "B7",
                "B1to7",
                "C1",
                "C2",
                "C3",
                "C4",
                "C1to4",
                "D"
            };

            foreach (string prop in propertiesToCheck)
            {
                if (oldVersion.TryGetValue(prop, out object objVal) && objVal is double val && !double.IsNaN(val))
                {
                    indicators[prop] = val;
                }
            }

            return indicators;
        }

        /***************************************************/
    }
}
