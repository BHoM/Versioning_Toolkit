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
using System.Collections.Generic;

namespace BH.Upgraders
{
    [Upgrader(9, 1)]
    public static class v91
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [VersioningTarget("BH.oM.Adapters.ElementRelationships.Persistence.LocationRelationship")]
        public static Dictionary<string, object> UpgradePersistenceLocationRelationship(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.oM.Adapters.ElementRelationships.Persistence.LocationRelationship, ElementRelationships_Adapter";

            Dictionary<string, object> relativeTransform = new Dictionary<string, object>();
            relativeTransform["_t"] = "BH.oM.ElementRelationships.RelativeTransform";
            relativeTransform["Transform"] = oldVersion["ExpectedTransform"];

            newVersion["ExpectedTransform"] = relativeTransform;
            newVersion["BHoM_Guid"] = oldVersion["BHoM_Guid"];
            newVersion["Link"] = oldVersion["Link"];
            newVersion["MappingGuid"] = oldVersion["MappingGuid"];
            newVersion["LastUpdated"] = oldVersion["LastUpdated"];
            newVersion["LastUpdatedBy"] = oldVersion["LastUpdatedBy"];

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.ElementRelationships.LocationRelationship")]
        public static Dictionary<string, object> UpgradeLocationRelationship(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.oM.ElementRelationships.LocationRelationship";

            Dictionary<string, object> relativeTransform = new Dictionary<string, object>();
            relativeTransform["_t"] = "BH.oM.ElementRelationships.RelativeTransform";
            relativeTransform["Transform"] = oldVersion["ExpectedTransform"];

            newVersion["ExpectedTransform"] = relativeTransform;
            newVersion["BHoM_Guid"] = oldVersion["BHoM_Guid"];
            newVersion["Link"] = oldVersion["Link"];
            newVersion["Mapping"] = oldVersion["Mapping"];

            return newVersion;
        }

        /***************************************************/
    }
}

