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

namespace BH.Upgrader.v42
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "4.1";

            ToNewObject.Add("BH.oM.Adapters.RAM.UniformLoadSet", UpgradeUniformLoadSet);
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/


        public static Dictionary<string, object> UpgradeUniformLoadSet(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>();

            newVersion["_t"] = "BH.oM.Structure.Loads.UniformLoadSet";
            newVersion["Name"] = oldVersion["Name"];
            newVersion["CustomData"] = oldVersion["CustomData"];
            newVersion["BHoM_Guid"] = oldVersion["BHoM_Guid"];
            newVersion["Tags"] = oldVersion["Tags"];
            newVersion["Fragments"] = oldVersion["Fragments"];

            object outObject;
            
            if ( oldVersion.TryGetValue("Loads", out outObject) )
            {
                Dictionary<string, object> oldLoadDict = outObject as Dictionary<string, object>;

                oldVersion.TryGetValue("Loadcases", out outObject);

                if (outObject != null)
                {
                    Dictionary<string, object> oldLoadcaseDict = outObject as Dictionary<string, object>;
                    List<string> names = oldLoadDict.Keys.ToList();

                    newVersion["Loads"] = names.Select(x => new Dictionary<string, object>()
                    {
                        { "_t", "BH.oM.Structure.Loads.UniformLoadSetRecord" },                   
                        { "Name", x },
                        { "Loadcase", oldLoadcaseDict[x] },
                        { "Load", oldLoadDict[x] },
                    }).ToList();
                }
                else
                {
                    List<string> names = oldLoadDict.Keys.ToList();

                    newVersion["Loads"] = names.Select(x => new Dictionary<string, object>()
                    {
                        { "_t", "BH.oM.Structure.Loads.UniformLoadSetRecord" },
                        { "Name", x },
                        { "Loadcase", null },
                        { "Load", oldLoadDict[x] },
                    }).ToList();
                }
            }

            return newVersion;
        }

        /***************************************************/
    }
}

