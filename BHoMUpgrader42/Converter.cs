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

            ToNewObject.Add("BH.oM.CFD.CFX.MonitorPoints", UpgradeMonitorPoints);
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, object> UpgradeMonitorPoints(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            List<string> coordinatesInputs = new List<string>();
            List<string> units = new List<string>();
            
            if (newVersion.ContainsKey("CoordinatesInputs"))
            {
                coordinatesInputs = (newVersion["CoordinatesInputs"] as object[]).Cast<string>().ToList();

                List<Dictionary<string, object>> points = new List<Dictionary<string, object>>(); 
                foreach (string coordinatesInput in coordinatesInputs)
                {
                    Dictionary<string, object> point = new Dictionary<string, object>();

                    double x = 0;
                    try
                    { x = Convert.ToDouble(coordinatesInput.Split(',')[0].Split('[')[0].Trim()); }
                    catch
                    // TODO provide warning here
                    { x = 0; }
                    units.Add(coordinatesInput.Split(',')[0].Split('[')[1].Split(']')[0].Trim());

                    double y = 0;
                    try
                    { y = Convert.ToDouble(coordinatesInput.Split(',')[1].Split('[')[0].Trim()); }
                    catch
                    // TODO provide warning here
                    { y = 0; }
                    units.Add(coordinatesInput.Split(',')[1].Split('[')[1].Split(']')[0].Trim());

                    double z = 0;
                    try
                    { z = Convert.ToDouble(coordinatesInput.Split(',')[2].Split('[')[0].Trim()); }
                    catch
                    // TODO provide warning here
                    { z = 0; }
                    units.Add(coordinatesInput.Split(',')[2].Split('[')[1].Split(']')[0].Trim());

                    point.Add("_t", "BH.oM.Geometry.Point");
                    point.Add("X", x);
                    point.Add("Y", y);
                    point.Add("Z", z);
                    points.Add(point);
                }
                newVersion.Add("Points", points);

                string unit = "";
                if (units.Distinct().Count() == 1)
                {
                    switch (units.Distinct().First())
                    {
                        case "cm":
                            unit = "Centimeters";
                            break;
                        case "ft":
                            unit = "Feet";
                            break;
                        case "in":
                            unit = "Inches";
                            break;
                        case "m":
                            unit = "Meters";
                            break;
                        case "mm":
                            unit = "Millimeters";
                            break;
                        default:
                            // TODO provide warning here
                            unit = "Meters";
                            break;
                    }
                }
                else
                    // TODO provide warning here
                    unit = "Meters";
                newVersion.Add("LengthUnit", unit);

                newVersion.Remove("CoordinatesInputs");
            }

            if (newVersion.ContainsKey("Variables"))
            {
                newVersion.Add("MonitorVariables", newVersion["Variables"]);
                newVersion.Remove("Variables");
            }

            newVersion.Add("Label", "");

            return newVersion;
        }

        /***************************************************/


        /***************************************************/
    }
}

