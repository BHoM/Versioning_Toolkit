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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgrader.v32
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "3.1";

            ToNewObject.Add("BH.oM.Structure.Results.BarDisplacement", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.BarDeformation", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.BarForce", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.BarResult", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.BarStrain", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.BarStress", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.CompositeUtilisation", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.SteelUtilisation", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.GlobalReactions", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.ModalDynamics", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.StructuralGlobalResult", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.MeshDisplacement", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.MeshForce", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.MeshElementResult", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.MeshResult", UpgradeMeshResult);
            ToNewObject.Add("BH.oM.Structure.Results.MeshStress", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.MeshVonMises", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.NodeAcceleration", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.NodeDisplacement", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.NodeReaction", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.NodeResult", UpgradeResult);
            ToNewObject.Add("BH.oM.Structure.Results.NodeVelocity", UpgradeResult);
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

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




