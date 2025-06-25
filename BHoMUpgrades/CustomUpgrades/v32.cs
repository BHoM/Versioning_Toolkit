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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgraders
{
    [Upgrader(3, 2)]
    public static class v32
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [VersioningTarget("BH.oM.Structure.Results.BarDisplacement")]
        [VersioningTarget("BH.oM.Structure.Results.BarDeformation")]
        [VersioningTarget("BH.oM.Structure.Results.BarForce")]
        [VersioningTarget("BH.oM.Structure.Results.BarResult")]
        [VersioningTarget("BH.oM.Structure.Results.BarStrain")]
        [VersioningTarget("BH.oM.Structure.Results.BarStress")]
        [VersioningTarget("BH.oM.Structure.Results.CompositeUtilisation")]
        [VersioningTarget("BH.oM.Structure.Results.SteelUtilisation")]
        [VersioningTarget("BH.oM.Structure.Results.GlobalReactions")]
        [VersioningTarget("BH.oM.Structure.Results.ModalDynamics")]
        [VersioningTarget("BH.oM.Structure.Results.StructuralGlobalResult")]
        [VersioningTarget("BH.oM.Structure.Results.MeshDisplacement")]
        [VersioningTarget("BH.oM.Structure.Results.MeshForce")]
        [VersioningTarget("BH.oM.Structure.Results.MeshElementResult")]
        [VersioningTarget("BH.oM.Structure.Results.MeshStress")]
        [VersioningTarget("BH.oM.Structure.Results.MeshVonMises")]
        [VersioningTarget("BH.oM.Structure.Results.NodeAcceleration")]
        [VersioningTarget("BH.oM.Structure.Results.NodeDisplacement")]
        [VersioningTarget("BH.oM.Structure.Results.NodeReaction")]
        [VersioningTarget("BH.oM.Structure.Results.NodeResult")]
        [VersioningTarget("BH.oM.Structure.Results.NodeVelocity")]
        public static Dictionary<string, object> UpgradeResult(Dictionary<string, object> result)
        {
            if (result == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(result);

            if (!newVersion.ContainsKey("ModeNumber"))
                newVersion.Add("ModeNumber", -1);

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.Structure.Results.MeshResult")]
        public static Dictionary<string, object> UpgradeMeshResult(Dictionary<string, object> result)
        {
            if (result == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(result);

            if (!newVersion.ContainsKey("ModeNumber"))
                newVersion.Add("ModeNumber", -1);

            //Serialisation_Engine is fallbacking to trying to de-serialise as CustomObject.
            //When doing so it might add GUID as a property, which needs to be removed if that is the case,
            //as MeshResult is not a BHoMObject and does not contain a BHoM_Guid.
            //This fallback only happens when versioning is needed on property of the object,
            //which is the case for the MeshResult that owns a list of MeshElementResults.
            if (newVersion.ContainsKey("BHoM_Guid"))
                newVersion.Remove("BHoM_Guid");

            return newVersion;
        }

        /***************************************************/

    }
}




