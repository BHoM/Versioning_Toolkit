/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
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
using System.Xml.Linq;

namespace BH.Upgrader.v70
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "6.3";

            ToNewObject.Add("BH.oM.Adaters.Lusas.LusasConfig", UpgradeLusasConfig);
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        public static Dictionary<string, object> UpgradeLusasConfig(Dictionary<string, object> old)
        {
            Dictionary<string, object> librarySettings = new Dictionary<string, object>();
            if (old.ContainsKey("LibrarySettings"))
            {
                librarySettings["_t"] = "BH.oM.Adapters.Lusas.LibrarySettings";
                librarySettings["LibrarySettings"] = old["LibrarySettings"];
            }

            return new Dictionary<string, object>
        {
            { "_t",  "BH.oM.Adapters.Lusas.LusasSettings" },
            { "MergeTolerance", double.NaN },
            { "LibrarySettings", librarySettings },
            { "WrapNonBHoMObjects", false },
            { "DefaultPushType", null},
            { "CloneBeforePush", true},
            { "DefaultPullType", null},
            { "HandleDependencies", true },
            { "UseAdapterId", true },
            { "UsaHashComparerAsDefault", true },
            { "ProcessInMemory", false },
            { "OnlyUpdateChangedObjects", true },
            { "CacheCRUDobjects", true },
            { "CreateOnly_DistinctObjects", false },
            { "CreateOnly_DistinctDependencies", true },
        };
        }
    }
}
