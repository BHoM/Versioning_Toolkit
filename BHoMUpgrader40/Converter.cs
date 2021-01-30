/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2021, the respective contributors. All rights reserved.
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

namespace BH.Upgrader.v40
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "3.3";

            ToNewObject.Add("BH.oM.Geometry.BoundaryRepresentation", UpgradeBoundaryRepresentation);
            ToNewObject.Add("BH.oM.Adapters.Revit.Parameters.RevitIdentifiers", UpgradeRevitIdentifiers);
            ToNewObject.Add("BH.oM.Geometry.ShapeProfiles.TaperedProfile", UpgradeTaperedProfile);
            ToNewObject.Add("BH.oM.Environment.Gains.Profile", UpgradeProfile);
            ToNewObject.Add("BH.oM.Environment.Elements.Space", UpgradeSpace);
            ToNewObject.Add("BH.oM.Structure.MaterialFragments.Aluminium", UpgradeMaterialFragment);
            ToNewObject.Add("BH.oM.Structure.MaterialFragments.Concrete", UpgradeMaterialFragment);
            ToNewObject.Add("BH.oM.Structure.MaterialFragments.GenericIsotropicMaterial", UpgradeMaterialFragment);
            ToNewObject.Add("BH.oM.Structure.MaterialFragments.GenericOrthotropicMaterial", UpgradeMaterialFragment);
            ToNewObject.Add("BH.oM.Structure.MaterialFragments.IMaterialFragment", UpgradeMaterialFragment);
            ToNewObject.Add("BH.oM.Structure.MaterialFragments.Steel", UpgradeMaterialFragment);
            ToNewObject.Add("BH.oM.Structure.MaterialFragments.Timber", UpgradeMaterialFragment);
            ToNewObject.Add("BH.oM.Adapters.Revit.FamilyLibrary", UpgradeFamilyLibrary);
            ToNewObject.Add("BH.oM.MEP.Elements.Node", UpgradeMEPNode);
            ToNewObject.Add("BH.oM.Diffing.DiffConfig", UpgradeDiffConfig);
            ToNewObject.Add("BH.oM.Diffing.HashFragment", UpgradeHashFragment);
            ToNewObject.Add("BH.oM.Adapters.Filing.PushConfig", UpgradeFilingPushConfig);
            ToNewObject.Add("BH.oM.Graphics.RenderMeshOptions", UpgradeRenderMeshOptions);
            ToNewObject.Add("BH.oM.Adapters.Revit.Elements.ViewPlan", UpgradeViewPlan);
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, object> UpgradeBoundaryRepresentation(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (!newVersion.ContainsKey("Volume"))
                newVersion.Add("Volume", double.NaN);

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeRevitIdentifiers(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (!newVersion.ContainsKey("OwnerViewId"))
                newVersion.Add("OwnerViewId", -1);

            if (!newVersion.ContainsKey("ParentElementId"))
                newVersion.Add("ParentElementId", -1);

            return newVersion;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeTaperedProfile(Dictionary<string, object> taperedProfile)
        {
            if (taperedProfile == null)
                return null;

            Dictionary<string, object> newTaperedProfile = new Dictionary<string, object>(taperedProfile);
            newTaperedProfile["_t"] = newTaperedProfile["_t"].ToString().Replace("Geometry", "Spatial");

            object profiles;
            newTaperedProfile.TryGetValue("Profiles", out profiles);
            Dictionary<string, object> profileDict = profiles as Dictionary<string, object>;
            Dictionary<string, object> updateProfileDict = new Dictionary<string, object>();
            foreach(KeyValuePair<string, object> profilePair in profileDict)
            {
                Dictionary<string, object> profileObject = profilePair.Value as Dictionary<string, object>;
                profileObject["_t"] = profileObject["_t"].ToString().Replace("Geometry", "Spatial");
                updateProfileDict.Add(profilePair.Key, profileObject);
            }

            newTaperedProfile["Profiles"] = updateProfileDict;

            if (!newTaperedProfile.ContainsKey("InterpolationOrder"))
            {
                List<string> keys = new List<string>(profileDict.Keys);
                newTaperedProfile.Add("InterpolationOrder", Enumerable.Repeat(1, keys.Count - 1).ToList());
            }

            return newTaperedProfile;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeProfile(Dictionary<string, object> profile)
        {
            if (profile == null)
                return null;

            Dictionary<string, object> newProfile = new Dictionary<string, object>(profile);
            newProfile["_t"] = newProfile["_t"].ToString().Replace("Gains", "SpaceCriteria");

            if (!newProfile["ProfileDay"].GetType().IsArray)
                newProfile["ProfileDay"] = new List<object> { profile["ProfileDay"] };

            return newProfile;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeSpace(Dictionary<string, object> space)
        {
            if (space == null || !space.ContainsKey("LightingGain") || 
                !space.ContainsKey("EquipmentGain") || !space.ContainsKey("PeopleGain") || 
                !space.ContainsKey("Infiltration") || !space.ContainsKey("Infiltration") || 
                !space.ContainsKey("Ventilation") || !space.ContainsKey("Exhaust"))
                return null;

            Dictionary<string, object> newSpace = new Dictionary<string, object>(space);

            foreach (Dictionary<string, object> prop in space.Values.OfType <Dictionary<string, object>>())
            {
                if (prop.ContainsKey("_t"))
                {
                    string t = prop["_t"] as string;
                    prop["_t"] = t.Replace("BH.oM.Environment.Gains", "BH.oM.Environment.SpaceCriteria")
                                  .Replace("BH.oM.Environment.Ventilation", "BH.oM.Environment.SpaceCriteria");
                }

                if (prop.ContainsKey("Profile"))
                    prop["Profile"] = UpgradeProfile(prop["Profile"] as Dictionary<string, object>);
            }

            List<string> propToFix = new List<string> { "LightingGain", "EquipmentGain", "PeopleGain", "Infiltration", "Ventilation", "Exhaust" };
            foreach (string prop in propToFix)
            {
                if (!space[prop].GetType().IsArray)
                    newSpace[prop] = new List<object> { space[prop] };
            }

            return newSpace;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeMaterialFragment(Dictionary<string, object> materialFragment)
        {
            if (materialFragment == null)
                return null;

            Dictionary<string, object> newMaterialFragment = new Dictionary<string, object>(materialFragment);

            if (newMaterialFragment.ContainsKey("EmbodiedCarbon"))
                newMaterialFragment.Remove("EmbodiedCarbon");

            return newMaterialFragment;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeFamilyLibrary(Dictionary<string, object> familyLibrary)
        {
            if (familyLibrary == null)
                return null;

            Dictionary<string, object> newFamilyLibrary = new Dictionary<string, object>(familyLibrary);

            if (newFamilyLibrary.ContainsKey("Dictionary"))
                newFamilyLibrary.Remove("Dictionary");
            
            return newFamilyLibrary;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeMEPNode(Dictionary<string, object> node)
        {
            if (node == null || !node.ContainsKey("Position"))
                return null;

            return node["Position"] as Dictionary<string, object>;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeDiffConfig(Dictionary<string, object> config)
        {
            if (config == null)
                return null;

            Dictionary<string, object> newConfig = new Dictionary<string, object> { { "_t", "BH.oM.Diffing.DiffingConfig" } };
            if (config.ContainsKey("EnablePropertyDiffing"))
                newConfig["EnablePropertyDiffing"] = config["EnablePropertyDiffing"];
            if (config.ContainsKey("MaxPropertyDifferences"))
                newConfig["MaxPropertyDifferences"] = config["MaxPropertyDifferences"];
            if (config.ContainsKey("StoreUnchangedObjects"))
                newConfig["IncludeUnchangedObjects"] = config["StoreUnchangedObjects"];

            Dictionary<string, object> comparison = new Dictionary<string, object> { { "_t", "BH.oM.Base.ComparisonConfig" } };
            if (config.ContainsKey("NumericTolerance"))
                comparison["NumericTolerance"] = config["NumericTolerance"];
            if (config.ContainsKey("PropertiesToConsider"))
                comparison["PropertiesToConsider"] = config["PropertiesToConsider"];
            if (config.ContainsKey("PropertiesToIgnore"))
                comparison["PropertyExceptions"] = config["PropertiesToIgnore"];
            if (config.ContainsKey("CustomDataToIgnore"))
                comparison["CustomdataKeysExceptions"] = config["CustomDataToIgnore"];
            newConfig["ComparisonConfig"] = comparison;

            return newConfig;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeHashFragment(Dictionary<string, object> hash)
        {
            if (hash == null)
                return null;

            Dictionary<string, object> newHash = new Dictionary<string, object> { { "_t", "BH.oM.Base.HashFragment" } };
            if (hash.ContainsKey("CurrentHash"))
                newHash["Hash"] = hash["CurrentHash"];

            return newHash;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeFilingPushConfig(Dictionary<string, object> config)
        {
            if (config == null)
                return null;

            Dictionary<string, object> newConfig = new Dictionary<string, object>(config);
            newConfig["_t"] = "BH.oM.Adapters.File.PushConfig";

            if (config.ContainsKey("DiffConfig"))
                newConfig["DiffingConfig"] = config["DiffConfig"];
            newConfig.Remove("DiffConfig");

            newConfig.Remove("DefaultFilePath");
            newConfig.Remove("AppendContent");

            return newConfig;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeRenderMeshOptions(Dictionary<string, object> options)
        {
            if (options == null)
                return null;

            Dictionary<string, object> newOptions = new Dictionary<string, object>(options);
            newOptions.Remove("CustomRendermeshKey");

            return newOptions;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeViewPlan(Dictionary<string, object> plan)
        {
            if (plan == null)
                return null;

            Dictionary<string, object> newPlan = new Dictionary<string, object>(plan);
            newPlan.Remove("IsTemplate");

            return newPlan;
        }

        /***************************************************/
    }
}

