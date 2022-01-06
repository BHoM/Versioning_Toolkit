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

namespace BH.Upgrader.v42
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "4.1";

            ToNewObject.Add("BH.oM.Adapters.RAM.UniformLoadSet", UpgradeUniformLoadSet);
            ToNewObject.Add("BH.oM.CFD.CFX.AutoTimescale", UpgradeAutoTimescale);
            ToNewObject.Add("BH.oM.LifeCycleAssessment.MaterialFragment.EnvironmentalProductDeclaration", UpgradeEnvironmentalProductDeclaration);
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/


        public static Dictionary<string, object> UpgradeUniformLoadSet(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>();

            newVersion["_t"] = "BH.oM.Structure.Loads.UniformLoadSet";
            newVersion["Name"] = oldVersion["Name"];
            newVersion["CustomData"] = oldVersion["CustomData"];
            newVersion["BHoM_Guid"] = oldVersion["BHoM_Guid"];
            newVersion["Tags"] = oldVersion["Tags"];
            newVersion["Fragments"] = oldVersion["Fragments"];

            object outObject;
            
            if ( oldVersion.TryGetValue("Loads", out outObject) )
            {
                Dictionary<string, object> oldLoadDict = outObject as Dictionary<string, object>;

                oldVersion.TryGetValue("Loadcases", out outObject);

                if (outObject != null)
                {
                    Dictionary<string, object> oldLoadcaseDict = outObject as Dictionary<string, object>;
                    List<string> names = oldLoadDict.Keys.ToList();

                    newVersion["Loads"] = names.Select(x => new Dictionary<string, object>()
                    {
                        { "_t", "BH.oM.Structure.Loads.UniformLoadSetRecord" },                   
                        { "Name", x },
                        { "Loadcase", oldLoadcaseDict[x] },
                        { "Load", oldLoadDict[x] },
                    }).ToList();
                }
                else
                {
                    List<string> names = oldLoadDict.Keys.ToList();

                    newVersion["Loads"] = names.Select(x => new Dictionary<string, object>()
                    {
                        { "_t", "BH.oM.Structure.Loads.UniformLoadSetRecord" },
                        { "Name", x },
                        { "Loadcase", null },
                        { "Load", oldLoadDict[x] },
                    }).ToList();
                }
            }

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeAutoTimescale(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            if (newVersion.ContainsKey("Factor") && newVersion["Factor"] != null)
                newVersion["Factor"] = newVersion["Factor"].ToString();

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeEnvironmentalProductDeclaration(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("AcidificationPotential"))
                newVersion.Remove("AcidificationPotential");

            if (newVersion.ContainsKey("BiogenicCarbon"))
                newVersion.Remove("BiogenicCarbon");

            if (newVersion.ContainsKey("Density"))
                newVersion.Remove("Density");

            if (newVersion.ContainsKey("DepletionOfAbioticResources"))
                newVersion.Remove("DepletionOfAbioticResources");

            if (newVersion.ContainsKey("DepletionOfAbioticResourcesFossilFuels"))
                newVersion.Remove("DepletionOfAbioticResourcesFossilFuels");

            if (newVersion.ContainsKey("Description"))
                newVersion.Remove("Description");

            if (newVersion.ContainsKey("EndOfLifeTreatment"))
                newVersion.Remove("EndOfLifeTreatment");

            if (newVersion.ContainsKey("EutrophicationPotential"))
                newVersion.Remove("EutrophicationPotential");

            if (newVersion.ContainsKey("ExportedElectricalEnergy"))
                newVersion.Remove("ExportedElectricalEnergy");

            if (newVersion.ContainsKey("ExportedThermalEnergy"))
                newVersion.Remove("ExportedThermalEnergy");

            if (newVersion.ContainsKey("FreshWater"))
                newVersion.Remove("FreshWater");

            if (newVersion.ContainsKey("GlobalWarmingPotential"))
                newVersion.Remove("GlobalWarmingPotential");

            if (newVersion.ContainsKey("HazardousWasteDisposed"))
                newVersion.Remove("HazardousWasteDisposed");

            if (newVersion.ContainsKey("IndustryStandards"))
                newVersion.Remove("IndustryStandards");

            if (newVersion.ContainsKey("LifeCycleAssessmentPhase"))
                newVersion.Remove("LifeCycleAssessmentPhase");

            if (newVersion.ContainsKey("Lifespan"))
                newVersion.Remove("Lifespan");

            if (newVersion.ContainsKey("LifeCycleAssessmentPhase"))
                newVersion.Remove("LifeCycleAssessmentPhase");

            if (newVersion.ContainsKey("Manufacturer"))
                newVersion.Remove("Manufacturer");

            if (newVersion.ContainsKey("MasterFormat"))
                newVersion.Remove("MasterFormat");

            if (newVersion.ContainsKey("MaterialForEnergyRecovery"))
                newVersion.Remove("MaterialForEnergyRecovery");

            if (newVersion.ContainsKey("NonHazardousWasteDisposed"))
                newVersion.Remove("NonHazardousWasteDisposed");

            if (newVersion.ContainsKey("NonRenewableSecondaryFuels"))
                newVersion.Remove("NonRenewableSecondaryFuels");

            if (newVersion.ContainsKey("OzoneDepletionPotential"))
                newVersion.Remove("OzoneDepletionPotential");

            if (newVersion.ContainsKey("PhotochemicalOzoneCreationPotential"))
                newVersion.Remove("PhotochemicalOzoneCreationPotential");

            if (newVersion.ContainsKey("Plant"))
                newVersion.Remove("Plant");

            if (newVersion.ContainsKey("PostalCode"))
                newVersion.Remove("PostalCode");

            if (newVersion.ContainsKey("PostConsumerRecycledContent"))
                newVersion.Remove("PostConsumerRecycledContent");

            if (newVersion.ContainsKey("PrimaryEnergyNonRenewableEnergy"))
                newVersion.Remove("PrimaryEnergyNonRenewableEnergy");

            if (newVersion.ContainsKey("PrimaryEnergyNonRenewableResource"))
                newVersion.Remove("PrimaryEnergyNonRenewableResource");

            if (newVersion.ContainsKey("PrimaryEnergyRenewableEnergy"))
                newVersion.Remove("PrimaryEnergyRenewableEnergy");

            if (newVersion.ContainsKey("PrimaryEnergyRenewableTotal"))
                newVersion.Remove("PrimaryEnergyRenewableTotal");

            if (newVersion.ContainsKey("PrimaryEnergyResourcesRawMaterials"))
                newVersion.Remove("PrimaryEnergyResourcesRawMaterials");

            if (newVersion.ContainsKey("RadioActiveWasteDisposed"))
                newVersion.Remove("RadioActiveWasteDisposed");

            if (newVersion.ContainsKey("ReferenceYear"))
                newVersion.Remove("ReferenceYear");

            if (newVersion.ContainsKey("RenewableSecondaryFuels"))
                newVersion.Remove("RenewableSecondaryFuels");

            if (newVersion.ContainsKey("SecondaryMaterial"))
                newVersion.Remove("SecondaryMaterial");

            if (newVersion.ContainsKey("Type"))
                newVersion.Add("_t", "BH.oM.LifeCycleAssessment.EPDType");

            if (newVersion.ContainsKey("EnvironmentalMetric"))
                newVersion.Add("_t", "System.Collections.Generic.List<BH.oM.LifeCycleAssessment.EnvironmentalMetric>");

            if (newVersion.ContainsKey("QuantityType"))
                newVersion.Add("_t", "BH.oM.LifeCycleAssessment.QuantityType");

            if (newVersion.ContainsKey("QuantityTypeValue"))
                newVersion.Add("_t", "System.Double");

            return newVersion;
        }

        /***************************************************/

    }
}


