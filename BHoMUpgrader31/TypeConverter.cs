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

namespace BH.Upgrader.v31
{
    public partial class Converter
    {
        /***************************************************/
        /**** Public Properties                         ****/
        /***************************************************/

        public Dictionary<string, string> ToNewType { get; set; } = new Dictionary<string, string>
        {

            { "BH.oM.Geometry.IElement", "BH.oM.Dimensional.IElement" },
            { "BH.oM.Geometry.IElement0D", "BH.oM.Dimensional.IElement0D" },
            { "BH.oM.Geometry.IElement1D", "BH.oM.Dimensional.IElement1D" },
            { "BH.oM.Geometry.IElement2D", "BH.oM.Dimensional.IElement2D" },
            { "BH.oM.Base.IBHoMFragment", "BH.oM.Base.IFragment" }
        };

        /***************************************************/

        public Dictionary<string, string> ToOldType { get; set; } = new Dictionary<string, string>
        {

            { "BH.oM.Dimensional.IElement", "BH.oM.Geometry.IElement" },
            { "BH.oM.Dimensional.IElement0D", "BH.oM.Geometry.IElement0D" },
            { "BH.oM.Dimensional.IElement1D", "BH.oM.Geometry.IElement1D" },
            { "BH.oM.Dimensional.IElement2D", "BH.oM.Geometry.IElement2D" },
            { "BH.oM.Base.IFragment", "BH.oM.Base.IBHoMFragment" }
        };

        /***************************************************/
    }
}
