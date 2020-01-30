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

namespace BH.Upgrader.v30
{
    public partial class Converter
    {
        /***************************************************/
        /**** Public Properties                         ****/
        /***************************************************/

        public Dictionary<string, MethodBase> ToNewMethod { get; set; } = new Dictionary<string, MethodBase>
        {
            {
                "BH.UI.Components.DeleteCaller.Delete(BH.Adapter.BHoMAdapter, BH.oM.Data.Requests.FilterRequest, System.Collections.Generic.Dictionary<System.String, System.Object>, System.Boolean)",
                typeof(BH.UI.Components.RemoveCaller).GetMethod("Remove")
            },
            {
                "BH.UI.Components.MoveCaller.Move(BH.Adapter.BHoMAdapter, BH.Adapter.BHoMAdapter, BH.oM.Data.Requests.IRequest, System.Collections.Generic.Dictionary<System.String, System.Object>, System.Collections.Generic.Dictionary<System.String, System.Object>, System.Boolean)",
                typeof(BH.UI.Components.MoveCaller).GetMethod("Move")
            },
            {
                "BH.UI.Components.PullCaller.Pull(BH.Adapter.BHoMAdapter, BH.oM.Data.Requests.IRequest, System.Collections.Generic.Dictionary<System.String, System.Object>, System.Boolean)",
                typeof(BH.UI.Components.PullCaller).GetMethod("Pull")
            },
            {
                "BH.UI.Components.PushCaller.Push(BH.Adapter.BHoMAdapter, System.Collections.Generic.IEnumerable<BH.oM.Base.IObject>, System.String, System.Collections.Generic.Dictionary<System.String, System.Object>, System.Boolean)",
                typeof(BH.UI.Components.PushCaller).GetMethod("Push")
            }
        };

        /***************************************************/

        public Dictionary<string, MethodBase> ToOldMethod { get; set; } = new Dictionary<string, MethodBase>
        {

        };

        /***************************************************/
    }
}
