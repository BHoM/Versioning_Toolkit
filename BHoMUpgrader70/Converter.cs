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

            ToNewObject.Add("BH.oM.Adapters.Lusas.LusasConfig", UpgradeLusasConfig);
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        public static Dictionary<string, object> UpgradeLusasConfig(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>();
            newVersion["_t"] = "BH.oM.Adapters.Lusas.LusasSettings";

            /*oldVersion.Remove("BHoM_Guid");
            oldVersion.Remove("Name");
            oldVersion.Remove("Fragments");
            oldVersion.Remove("Tags");
            oldVersion.Remove("CustomData");*/

            newVersion["MergeTolerance"] = double.NaN;
            newVersion["LibrarySettings"] = oldVersion["LibrarySettings"];
            newVersion["WrapNonBHoMObjects"] = false;
            newVersion["DefaultPushType"] = 0;
            newVersion["CloneBeforePush"] = true;
            newVersion["DefaultPullType"] = 0;
            newVersion["HandleDependencies"] = true;
            newVersion["UseAdapterId"] = true;
            newVersion["UseHashComparerAsDefault"] = true;
            newVersion["ProcessInMemory"] = false;
            newVersion["OnlyUpdateChangedObjects"] = true;
            newVersion["CacheCRUDobjects"] = true;
            newVersion["CreateOnly_DistinctObjects"] = false;
            newVersion["CreateOnly_DistinctDependencies"] = true;

            if (newVersion.ContainsKey("BHoM_Guid"))
                newVersion.Remove("BHoM_Guid");

            return newVersion;
        }
    }
}
