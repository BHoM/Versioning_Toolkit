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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgrader.v31
{
    public partial class Converter
    {
        /***************************************************/
        /**** Public Properties                         ****/
        /***************************************************/

        public Dictionary<string, MethodBase> ToNewMethod { get; set; } = new Dictionary<string, MethodBase>
        {
            {
                "BH.Engine.Geometry.Compute.ClipPolylines(BH.oM.Geometry.Polyline, BH.oM.Geometry.Polyline)",
                typeof(BH.Engine.Geometry.Compute).GetMethod("BooleanIntersection", new Type[] { typeof(BH.oM.Geometry.Polyline), typeof(BH.oM.Geometry.Polyline), typeof(double) })
            },
        };

        /***************************************************/

        public Dictionary<string, MethodBase> ToOldMethod { get; set; } = new Dictionary<string, MethodBase>
        {

        };

        /***************************************************/
    }
}
