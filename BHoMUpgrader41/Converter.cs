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

namespace BH.Upgrader.v41
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "4.0";

            ToNewObject.Add("BH.oM.Programming.DataParam", UpdateNodeParam);
            ToNewObject.Add("BH.oM.Programming.ReceiverParam", UpdateNodeParam);
            ToNewObject.Add("BH.oM.Test.Results.TestResult", UpdateTestResult);
            ToNewObject.Add("BH.oM.Adapters.Revit.Parameters.RevitIdentifiers", UpgradeRevitIdentifiers);
            ToNewObject.Add("BH.oM.MEP.Fixtures.AirTerminal", UpgradeAirTerminalUnit);
            ToNewObject.Add("BH.oM.MEP.Fixtures.LightFixture", UpgradeLightFixture);
            ToNewObject.Add("BH.oM.MEP.System.Dampers.VolumeDamper", UpgradeVolumeDamper);
            ToNewObject.Add("BH.oM.MEP.Equipment.MechanicalEquipment", UpgradeMechanicalEquipment);
            ToNewObject.Add("BH.oM.CFD.Harpoon.HarpoonObject", UpgradeHarpoonObject);
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, object> UpgradeMechanicalEquipment(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("Position"))
            {
                newVersion.Add("Location", UpgradeMEPPointToMEPNode(newVersion["Position"] as Dictionary<string, object>));
                newVersion.Remove("Position");
            }

            if (!newVersion.ContainsKey("OrientationAngle"))
                newVersion.Add("OrientationAngle", 0);

            if (!newVersion.ContainsKey("Power"))
                newVersion.Add("Power", 0);

            if (!newVersion.ContainsKey("FlowRate"))
                newVersion.Add("FlowRate", 0);

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeVolumeDamper(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("Location"))
                newVersion["Location"] = UpgradeMEPPointToMEPNode(newVersion["Location"] as Dictionary<string, object>);

            if (!newVersion.ContainsKey("OrientationAngle"))
                newVersion.Add("OrientationAngle", 0);

            if (newVersion.ContainsKey("InletDuctProperties"))
                newVersion.Remove("InletDuctProperties");

            if (newVersion.ContainsKey("OutletDuctProperties"))
                newVersion.Remove("OutletDuctProperties");

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeLightFixture(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("Position"))
            {
                newVersion.Add("Location", UpgradeMEPPointToMEPNode(newVersion["Position"] as Dictionary<string, object>));
                newVersion.Remove("Position");
            }

            if (!newVersion.ContainsKey("OrientationAngle"))
                newVersion.Add("OrientationAngle", 0);

            if (!newVersion.ContainsKey("Power"))
                newVersion.Add("Power", 0);

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeAirTerminalUnit(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if(newVersion.ContainsKey("FlowRate"))
            {
                newVersion.Add("AirFlowRate", newVersion["FlowRate"]);
                newVersion.Remove("FlowRate");
            }

            if(newVersion.ContainsKey("Position"))
            {
                newVersion.Add("Location", UpgradeMEPPointToMEPNode(newVersion["Position"] as Dictionary<string, object>));
                newVersion.Remove("Position");
            }

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeMEPPointToMEPNode(Dictionary<string, object> mepPoint)
        {
            if (mepPoint == null)
                return null;

            if (mepPoint.ContainsKey("_t") && (mepPoint["_t"] as string) == "BH.oM.MEP.System.Node")
                return mepPoint; //Already a node, doesn't need converting

            Dictionary<string, object> node = new Dictionary<string, object>();
            node.Add("_t", "BH.oM.MEP.System.Node");
            node.Add("Position", mepPoint);

            return node;
        }

        /***************************************************/

        private static Dictionary<string, object> UpdateNodeParam(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            newVersion.Remove("ParentId");

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpdateTestResult(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            if (oldVersion.ContainsKey("Status"))
            {
                switch (oldVersion["Status"].ToString())
                {
                    case "Undefined":
                    case "CriticalFail":
                        newVersion["Status"] = "Error";
                        break;
                    case "Fail":
                        newVersion["Status"] = "Warning";
                        break;
                    case "Pass":
                        newVersion["Status"] = "Pass";
                        break;
                    default:
                        break;
                }
            }

            if (oldVersion.ContainsKey("Events") && oldVersion["Events"] is object[])
            {
                IEnumerable<Dictionary<string, object>> events = (oldVersion["Events"] as object[]).OfType<Dictionary<string, object>>();

                newVersion.Remove("Events");
                newVersion["Information"] = events.Select(oldEvent =>
                {
                    Dictionary<string, object> newInfo = new Dictionary<string, object>();
                    newInfo["_t"] = "BH.oM.Test.Results.EventMessage";

                    if (oldEvent.ContainsKey("Message"))
                        newInfo["Message"] = oldEvent["Message"];

                    if (oldEvent.ContainsKey("UtcTime"))
                        newInfo["UTCTime"] = oldEvent["UtcTime"];

                    if (oldEvent.ContainsKey("StackTrace"))
                        newInfo["StackTrace"] = oldEvent["StackTrace"];

                    if (oldEvent.ContainsKey("Type"))
                    {
                        switch (oldEvent["Type"].ToString())
                        {
                            case "Unknown":
                            case "Error":
                                newInfo["Status"] = "Error";
                                break;
                            case "Warning":
                                newInfo["Status"] = "Warning";
                                break;
                            case "Note":
                                newInfo["Status"] = "Pass";
                                break;
                            default:
                                break;
                        }
                    }

                    return newInfo;
                }).ToList();
            }

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeRevitIdentifiers(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (!newVersion.ContainsKey("Workset"))
                newVersion.Add("Workset", "");

            if (!newVersion.ContainsKey("HostId"))
                newVersion.Add("HostId", -1);

            if (!newVersion.ContainsKey("LinkPath"))
                newVersion.Add("LinkPath", "");

            return newVersion;
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeHarpoonObject(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            if (newVersion.ContainsKey("Center"))
            {
                if (newVersion["Center"] == null)
                {
                    newVersion["_t"] = newVersion["_t"].ToString().Replace("HarpoonObject", "SurfaceHarpoonObject");
                    newVersion.Remove("Center");
                }
                else
                {
                    newVersion["_t"] = newVersion["_t"].ToString().Replace("HarpoonObject", "VolumeHarpoonObject");
                    newVersion.Add("Location", newVersion["Center"]);
                    newVersion.Remove("Center");
                }
            }

            return newVersion;
        }

        /***************************************************/
    }
}

