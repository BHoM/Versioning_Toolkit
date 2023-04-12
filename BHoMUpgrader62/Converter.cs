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

namespace BH.Upgrader.v62
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "6.1";

            ToNewObject.Add("BH.oM.Lighting.Elements.Luminaire", UpgradeLuminaire);
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, object> UpgradeLuminaire(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            if (newVersion.ContainsKey("Direction"))
            {
                if (newVersion["Direction"] is null)
                {
                    newVersion["Orientation"] = null;
                    newVersion.Remove("Direction");
                    return newVersion;
                }

                Dictionary<string, object> xVec = new Dictionary<string, object>() { ["X"] = 1, ["Y"] = 0, ["Z"] = 0 };
                Dictionary<string, object> yVec = new Dictionary<string, object>() { ["X"] = 0, ["Y"] = 1, ["Z"] = 0 };
                Dictionary<string, object> zVec = new Dictionary<string, object>() { ["X"] = 0, ["Y"] = 0, ["Z"] = 1 };

                Dictionary<string, object> basis = new Dictionary<string, object>() { ["X"] = xVec, ["Y"] = yVec, ["Z"] = zVec };

                Dictionary<string, object> orientation = newVersion["Direction"] as Dictionary<string, object>;
                double dirX = (double)orientation["X"];
                double dirY = (double)orientation["Y"];
                double dirZ = (double)orientation["Z"];

                if (dirX == 0 && dirY == 0)
                {
                    if (dirZ == 0)
                    {
                        basis = null;
                    }
                    else if (dirZ > 0)
                    {
                        basis["X"] = xVec;
                        basis["Y"] = yVec;
                        basis["Z"] = zVec;
                    }
                    else
                    {
                        basis["X"] = new Dictionary<string, object>() { ["X"] = -1, ["Y"] = 0, ["Z"] = 0 };
                        basis["Y"] = yVec;
                        basis["Z"] = new Dictionary<string, object>() { ["X"] = 0, ["Y"] = 0, ["Z"] = -1 };
                    }
                }
                else
                {
                    basis["X"] = Normalise(CrossProduct(orientation, zVec));
                    basis["Y"] = Normalise(CrossProduct(orientation, xVec));
                    basis["Z"] = orientation;
                }

                newVersion["Orientation"] = basis;
                newVersion.Remove("Direction");
            }
            return newVersion;
        }

        private static Dictionary<string, object> CrossProduct(Dictionary<string, object> a, Dictionary<string, object> b)
        {
            double aX = (double)a["X"];
            double aY = (double)a["Y"];
            double aZ = (double)a["Z"];

            double bX = (double)b["X"];
            double bY = (double)b["Y"];
            double bZ = (double)b["Z"];

            return new Dictionary<string, object> { ["X"] = aY * bZ - aZ * bY, ["Y"] = aZ * bX - aX * bZ, ["Z"] = aX * bY - aY * bX };
        }

        private static Dictionary<string, object> Normalise(Dictionary<string, object> a)
        {
            double x = (double)a["X"];
            double y = (double)a["Y"];
            double z = (double)a["Z"];
            double d = Math.Sqrt(x * x + y * y + z * z);

            if (d == 0)
                return null;

            return new Dictionary<string, object> { ["X"] = x / d, ["Y"] = y / d, ["Z"] = z / d };

        }
    }
}

