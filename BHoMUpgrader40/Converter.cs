/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
            ToNewObject.Add("BH.oM.Environment.Elements.Space", UpgradeSpace);
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

        public static Dictionary<string, object> UpgradeSpace(Dictionary<string, object> space)
        {
            if (space == null)
                return null;

            Dictionary<string, object> newSpace = new Dictionary<string, object>(space);
            newSpace["LightingGain"] = new List<object> { space["LightingGain"] };
            newSpace["EquipmentGain"] = new List<object> { space["EquipmentGain"] };
            newSpace["PeopleGain"] = new List<object> { space["PeopleGain"] };
            newSpace["Infiltration"] = new List<object> { space["Infiltration"] };
            newSpace["Ventilation"] = new List<object> { space["Ventilation"] };
            newSpace["Exhaust"] = new List<object> { space["Exhaust"] };

            return newSpace;
        }
    }
}
