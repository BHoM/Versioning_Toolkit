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
            {
            "BH.Engine.Common.Compute.DistributeOutlines(System.Collections.Generic.List<System.Collections.Generic.List<BH.oM.Dimensional.IElement1D>>, System.Boolean, System.Double)",
            typeof(BH.Engine.Spatial.Compute).GetMethod("DistributeOutlines", new Type[] {typeof(System.Collections.Generic.List<System.Collections.Generic.List<BH.oM.Dimensional.IElement1D>>), typeof(System.Boolean), typeof(System.Double)})
            },
            {
            "BH.Engine.Common.Modify.Translate(BH.oM.Dimensional.IElement2D, BH.oM.Geometry.Vector)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("Translate", new Type[] {typeof(BH.oM.Dimensional.IElement2D), typeof(BH.oM.Geometry.Vector)})
            },
            {
            "BH.Engine.Common.Modify.Translate(BH.oM.Dimensional.IElement1D, BH.oM.Geometry.Vector)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("Translate", new Type[] {typeof(BH.oM.Dimensional.IElement1D), typeof(BH.oM.Geometry.Vector)})
            },
            {
            "BH.Engine.Common.Modify.Translate(BH.oM.Dimensional.IElement0D, BH.oM.Geometry.Vector)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("Translate", new Type[] {typeof(BH.oM.Dimensional.IElement0D), typeof(BH.oM.Geometry.Vector)})
            },
            {
            "BH.Engine.Common.Modify.ISetElements0D(BH.oM.Dimensional.IElement1D, System.Collections.Generic.List<BH.oM.Dimensional.IElement0D>)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("ISetElements0D", new Type[] {typeof(BH.oM.Dimensional.IElement1D), typeof(System.Collections.Generic.List<BH.oM.Dimensional.IElement0D>)})
            },
            {
            "BH.Engine.Common.Modify.ISetGeometry(BH.oM.Dimensional.IElement0D, BH.oM.Geometry.Point)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("ISetGeometry", new Type[] {typeof(BH.oM.Dimensional.IElement0D), typeof(BH.oM.Geometry.Point)})
            },
            {
            "BH.Engine.Common.Modify.ISetGeometry(BH.oM.Dimensional.IElement1D, BH.oM.Geometry.ICurve)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("ISetGeometry", new Type[] {typeof(BH.oM.Dimensional.IElement1D), typeof(BH.oM.Geometry.ICurve)})
            },
            {
            "BH.Engine.Common.Modify.ISetInternalElements2D(BH.oM.Dimensional.IElement2D, System.Collections.Generic.List<BH.oM.Dimensional.IElement2D>)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("ISetInternalElements2D", new Type[] {typeof(BH.oM.Dimensional.IElement2D), typeof(System.Collections.Generic.List<BH.oM.Dimensional.IElement2D>)})
            },
            {
            "BH.Engine.Common.Modify.ISetOutlineElements1D(BH.oM.Dimensional.IElement2D, System.Collections.Generic.List<BH.oM.Dimensional.IElement1D>)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("ISetOutlineElements1D", new Type[] {typeof(BH.oM.Dimensional.IElement2D), typeof(System.Collections.Generic.List<BH.oM.Dimensional.IElement1D>)})
            },
            {
            "BH.Engine.Common.Query.Area(BH.oM.Dimensional.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Area", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.Bounds(BH.oM.Dimensional.IElement0D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Bounds", new Type[] {typeof(BH.oM.Dimensional.IElement0D)})
            },
            {
            "BH.Engine.Common.Query.Bounds(BH.oM.Dimensional.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Bounds", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.Bounds(BH.oM.Dimensional.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Bounds", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IBounds(BH.oM.Dimensional.IElement)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IBounds", new Type[] {typeof(BH.oM.Dimensional.IElement)})
            },
            {
            "BH.Engine.Common.Query.IBounds(System.Collections.Generic.IEnumerable<BH.oM.Dimensional.IElement>)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IBounds", new Type[] {typeof(System.Collections.Generic.IEnumerable<BH.oM.Dimensional.IElement>)})
            },
            {
            "BH.Engine.Common.Query.Centroid(BH.oM.Dimensional.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Centroid", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.Centroid(BH.oM.Dimensional.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Centroid", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.ControlPoints(BH.oM.Dimensional.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("ControlPoints", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.ControlPoints(BH.oM.Dimensional.IElement2D, System.Boolean)",
            typeof(BH.Engine.Spatial.Query).GetMethod("ControlPoints", new Type[] {typeof(BH.oM.Dimensional.IElement2D), typeof(System.Boolean)})
            },
            {
            "BH.Engine.Common.Query.ElementCurves(BH.oM.Dimensional.IElement1D, System.Boolean)",
            typeof(BH.Engine.Spatial.Query).GetMethod("ElementCurves", new Type[] {typeof(BH.oM.Dimensional.IElement1D), typeof(System.Boolean)})
            },
            {
            "BH.Engine.Common.Query.ElementCurves(BH.oM.Dimensional.IElement2D, System.Boolean)",
            typeof(BH.Engine.Spatial.Query).GetMethod("ElementCurves", new Type[] {typeof(BH.oM.Dimensional.IElement2D), typeof(System.Boolean)})
            },
            {
            "BH.Engine.Common.Query.IElementCurves(BH.oM.Dimensional.IElement, System.Boolean)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IElementCurves", new Type[] {typeof(BH.oM.Dimensional.IElement), typeof(System.Boolean)})
            },
            {
            "BH.Engine.Common.Query.IElementCurves(System.Collections.Generic.IEnumerable<BH.oM.Dimensional.IElement>, System.Boolean)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IElementCurves", new Type[] {typeof(System.Collections.Generic.IEnumerable<BH.oM.Dimensional.IElement>), typeof(System.Boolean)})
            },
            {
            "BH.Engine.Common.Query.ElementVertices(BH.oM.Dimensional.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("ElementVertices", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.ElementVertices(BH.oM.Dimensional.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("ElementVertices", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IElementVertices(BH.oM.Dimensional.IElement)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IElementVertices", new Type[] {typeof(BH.oM.Dimensional.IElement)})
            },
            {
            "BH.Engine.Common.Query.IElementVertices(System.Collections.Generic.IEnumerable<BH.oM.Dimensional.IElement>)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IElementVertices", new Type[] {typeof(System.Collections.Generic.IEnumerable<BH.oM.Dimensional.IElement>)})
            },
            {
            "BH.Engine.Common.Query.IElements0D(BH.oM.Dimensional.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IElements0D", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.IGeometry(BH.oM.Dimensional.IElement0D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IGeometry", new Type[] {typeof(BH.oM.Dimensional.IElement0D)})
            },
            {
            "BH.Engine.Common.Query.IGeometry(BH.oM.Dimensional.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IGeometry", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.IInternalElements2D(BH.oM.Dimensional.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IInternalElements2D", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IInternalOutlineCurves(BH.oM.Dimensional.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IInternalOutlineCurves", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IsSelfIntersecting(BH.oM.Dimensional.IElement1D, System.Double)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IsSelfIntersecting", new Type[] {typeof(BH.oM.Dimensional.IElement1D), typeof(System.Double)})
            },
            {
            "BH.Engine.Common.Query.IsSelfIntersecting(BH.oM.Dimensional.IElement2D, System.Double)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IsSelfIntersecting", new Type[] {typeof(BH.oM.Dimensional.IElement2D), typeof(System.Double)})
            },
            {
            "BH.Engine.Common.Query.Length(BH.oM.Dimensional.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Length", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.Normal(BH.oM.Dimensional.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Normal", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IOutlineElements1D(BH.oM.Dimensional.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IOutlineElements1D", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IOutlineCurve(BH.oM.Dimensional.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IOutlineCurve", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IOutlineCurve(System.Collections.Generic.List<BH.oM.Dimensional.IElement1D>)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IOutlineCurve", new Type[] {typeof(System.Collections.Generic.List<BH.oM.Dimensional.IElement1D>)})
            },


            {
            "BH.Engine.Common.Compute.DistributeOutlines(System.Collections.Generic.List<System.Collections.Generic.List<BH.oM.Geometry.IElement1D>>, System.Boolean, System.Double)",
            typeof(BH.Engine.Spatial.Compute).GetMethod("DistributeOutlines", new Type[] {typeof(System.Collections.Generic.List<System.Collections.Generic.List<BH.oM.Dimensional.IElement1D>>), typeof(System.Boolean), typeof(System.Double)})
            },
            {
            "BH.Engine.Common.Modify.Translate(BH.oM.Geometry.IElement2D, BH.oM.Geometry.Vector)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("Translate", new Type[] {typeof(BH.oM.Dimensional.IElement2D), typeof(BH.oM.Geometry.Vector)})
            },
            {
            "BH.Engine.Common.Modify.Translate(BH.oM.Geometry.IElement1D, BH.oM.Geometry.Vector)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("Translate", new Type[] {typeof(BH.oM.Dimensional.IElement1D), typeof(BH.oM.Geometry.Vector)})
            },
            {
            "BH.Engine.Common.Modify.Translate(BH.oM.Geometry.IElement0D, BH.oM.Geometry.Vector)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("Translate", new Type[] {typeof(BH.oM.Dimensional.IElement0D), typeof(BH.oM.Geometry.Vector)})
            },
            {
            "BH.Engine.Common.Modify.ISetElements0D(BH.oM.Geometry.IElement1D, System.Collections.Generic.List<BH.oM.Geometry.IElement0D>)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("ISetElements0D", new Type[] {typeof(BH.oM.Dimensional.IElement1D), typeof(System.Collections.Generic.List<BH.oM.Dimensional.IElement0D>)})
            },
            {
            "BH.Engine.Common.Modify.ISetGeometry(BH.oM.Geometry.IElement0D, BH.oM.Geometry.Point)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("ISetGeometry", new Type[] {typeof(BH.oM.Dimensional.IElement0D), typeof(BH.oM.Geometry.Point)})
            },
            {
            "BH.Engine.Common.Modify.ISetGeometry(BH.oM.Geometry.IElement1D, BH.oM.Geometry.ICurve)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("ISetGeometry", new Type[] {typeof(BH.oM.Dimensional.IElement1D), typeof(BH.oM.Geometry.ICurve)})
            },
            {
            "BH.Engine.Common.Modify.ISetInternalElements2D(BH.oM.Geometry.IElement2D, System.Collections.Generic.List<BH.oM.Geometry.IElement2D>)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("ISetInternalElements2D", new Type[] {typeof(BH.oM.Dimensional.IElement2D), typeof(System.Collections.Generic.List<BH.oM.Dimensional.IElement2D>)})
            },
            {
            "BH.Engine.Common.Modify.ISetOutlineElements1D(BH.oM.Geometry.IElement2D, System.Collections.Generic.List<BH.oM.Geometry.IElement1D>)",
            typeof(BH.Engine.Spatial.Modify).GetMethod("ISetOutlineElements1D", new Type[] {typeof(BH.oM.Dimensional.IElement2D), typeof(System.Collections.Generic.List<BH.oM.Dimensional.IElement1D>)})
            },
            {
            "BH.Engine.Common.Query.Area(BH.oM.Geometry.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Area", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.Bounds(BH.oM.Geometry.IElement0D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Bounds", new Type[] {typeof(BH.oM.Dimensional.IElement0D)})
            },
            {
            "BH.Engine.Common.Query.Bounds(BH.oM.Geometry.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Bounds", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.Bounds(BH.oM.Geometry.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Bounds", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IBounds(BH.oM.Geometry.IElement)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IBounds", new Type[] {typeof(BH.oM.Dimensional.IElement)})
            },
            {
            "BH.Engine.Common.Query.IBounds(System.Collections.Generic.IEnumerable<BH.oM.Geometry.IElement>)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IBounds", new Type[] {typeof(System.Collections.Generic.IEnumerable<BH.oM.Dimensional.IElement>)})
            },
            {
            "BH.Engine.Common.Query.Centroid(BH.oM.Geometry.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Centroid", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.Centroid(BH.oM.Geometry.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Centroid", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.ControlPoints(BH.oM.Geometry.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("ControlPoints", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.ControlPoints(BH.oM.Geometry.IElement2D, System.Boolean)",
            typeof(BH.Engine.Spatial.Query).GetMethod("ControlPoints", new Type[] {typeof(BH.oM.Dimensional.IElement2D), typeof(System.Boolean)})
            },
            {
            "BH.Engine.Common.Query.ElementCurves(BH.oM.Geometry.IElement1D, System.Boolean)",
            typeof(BH.Engine.Spatial.Query).GetMethod("ElementCurves", new Type[] {typeof(BH.oM.Dimensional.IElement1D), typeof(System.Boolean)})
            },
            {
            "BH.Engine.Common.Query.ElementCurves(BH.oM.Geometry.IElement2D, System.Boolean)",
            typeof(BH.Engine.Spatial.Query).GetMethod("ElementCurves", new Type[] {typeof(BH.oM.Dimensional.IElement2D), typeof(System.Boolean)})
            },
            {
            "BH.Engine.Common.Query.IElementCurves(BH.oM.Geometry.IElement, System.Boolean)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IElementCurves", new Type[] {typeof(BH.oM.Dimensional.IElement), typeof(System.Boolean)})
            },
            {
            "BH.Engine.Common.Query.IElementCurves(System.Collections.Generic.IEnumerable<BH.oM.Geometry.IElement>, System.Boolean)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IElementCurves", new Type[] {typeof(System.Collections.Generic.IEnumerable<BH.oM.Dimensional.IElement>), typeof(System.Boolean)})
            },
            {
            "BH.Engine.Common.Query.ElementVertices(BH.oM.Geometry.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("ElementVertices", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.ElementVertices(BH.oM.Geometry.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("ElementVertices", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IElementVertices(BH.oM.Geometry.IElement)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IElementVertices", new Type[] {typeof(BH.oM.Dimensional.IElement)})
            },
            {
            "BH.Engine.Common.Query.IElementVertices(System.Collections.Generic.IEnumerable<BH.oM.Geometry.IElement>)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IElementVertices", new Type[] {typeof(System.Collections.Generic.IEnumerable<BH.oM.Dimensional.IElement>)})
            },
            {
            "BH.Engine.Common.Query.IElements0D(BH.oM.Geometry.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IElements0D", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.IGeometry(BH.oM.Geometry.IElement0D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IGeometry", new Type[] {typeof(BH.oM.Dimensional.IElement0D)})
            },
            {
            "BH.Engine.Common.Query.IGeometry(BH.oM.Geometry.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IGeometry", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.IInternalElements2D(BH.oM.Geometry.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IInternalElements2D", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IInternalOutlineCurves(BH.oM.Geometry.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IInternalOutlineCurves", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IsSelfIntersecting(BH.oM.Geometry.IElement1D, System.Double)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IsSelfIntersecting", new Type[] {typeof(BH.oM.Dimensional.IElement1D), typeof(System.Double)})
            },
            {
            "BH.Engine.Common.Query.IsSelfIntersecting(BH.oM.Geometry.IElement2D, System.Double)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IsSelfIntersecting", new Type[] {typeof(BH.oM.Dimensional.IElement2D), typeof(System.Double)})
            },
            {
            "BH.Engine.Common.Query.Length(BH.oM.Geometry.IElement1D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Length", new Type[] {typeof(BH.oM.Dimensional.IElement1D)})
            },
            {
            "BH.Engine.Common.Query.Normal(BH.oM.Geometry.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("Normal", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IOutlineElements1D(BH.oM.Geometry.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IOutlineElements1D", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IOutlineCurve(BH.oM.Geometry.IElement2D)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IOutlineCurve", new Type[] {typeof(BH.oM.Dimensional.IElement2D)})
            },
            {
            "BH.Engine.Common.Query.IOutlineCurve(System.Collections.Generic.List<BH.oM.Geometry.IElement1D>)",
            typeof(BH.Engine.Spatial.Query).GetMethod("IOutlineCurve", new Type[] {typeof(System.Collections.Generic.List<BH.oM.Dimensional.IElement1D>)})
            }
        };

        /***************************************************/

        public Dictionary<string, MethodBase> ToOldMethod { get; set; } = new Dictionary<string, MethodBase>
        {

        };

        /***************************************************/
    }
}
