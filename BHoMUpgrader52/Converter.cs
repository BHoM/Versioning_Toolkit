/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2024, the respective contributors. All rights reserved.
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

namespace BH.Upgrader.v52
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "5.1";

            ToNewObject.Add("BH.oM.Structure.Loads.TributaryRegion", UpgradeTributaryRegion);
            ToNewObject.Add("BH.oM.Lighting.Results.Illuminance.Lux", UpgradeLux);
            ToNewObject.Add("BH.oM.Lighting.Results.Mesh.MeshElementResult", UpgradeLightingMeshElementResult);
            ToNewObject.Add("BH.oM.Lighting.Results.Mesh.MeshResult", UpgradeLightingMeshResult);
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        public static Dictionary<string, object> UpgradeTributaryRegion(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            newVersion["Regions"] = "[]";
            if (newVersion.ContainsKey("Region"))
            {
                newVersion["Regions"] = new List<object> { newVersion["Region"] };
                newVersion.Remove("Region");
            }
            else
            {
                newVersion["Regions"] = new List<object>();
            }

            if (!newVersion.ContainsKey("SupportingGuid"))
                newVersion["SupportingGuid"] = Guid.Empty;

            if (!newVersion.ContainsKey("Property"))
                newVersion["Property"] = null;

            return newVersion;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeLightingMeshResult(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("TimeStep"))
                newVersion.Remove("TimeStep");

            return newVersion;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeLux(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = UpgradeLightingMeshElementResult(oldVersion);

            if (oldVersion.ContainsKey("LuxLevel"))
                newVersion["LuxLevel"] = new List<object> { oldVersion["LuxLevel"] };
            else
                newVersion["LuxLevel"] = new List<object>();

            return newVersion;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeLightingMeshElementResult(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (!newVersion.ContainsKey("MeshFaceId"))
                newVersion["MeshFaceId"] = null;

            if (newVersion.ContainsKey("NodeID"))
            {
                newVersion["NodeId"] = newVersion["NodeID"];
                newVersion.Remove("NodeID");
            }
            else
                newVersion["NodeId"] = null;

            if (newVersion.ContainsKey("TimeStep"))
                newVersion.Remove("TimeStep");

            return newVersion;
        }

        /***************************************************/

    }
}


