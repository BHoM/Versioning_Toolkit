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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Upgrader.v30
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "";

            ToNewObject.Add("BH.oM.Versioning.OldVersion", UpgradeOldVersion); // For the change of class name
            ToNewObject.Add("BH.oM.Versioning.NewVersion", UpgradeOldVersion); // For the change of properties
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/


        public static Dictionary<string, object> UpgradeOldVersion(Dictionary<string, object> version)
        {
            if (version == null)
                return null;

            double A = 0;
            if (version.ContainsKey("A")) 
                A = (double)version["A"];

            double B = 0;
            if (version.ContainsKey("B"))
                B = (double)version["B"];

            return new Dictionary<string, object>
            {
                { "_t",  "BH.oM.Versioning.NewVersion" },
                { "AplusB", A + B },
                { "AminusB", A - B }
            };
        }

        /***************************************************/
    }
}

