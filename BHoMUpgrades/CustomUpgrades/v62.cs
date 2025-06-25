/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2025, the respective contributors. All rights reserved.
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

using BH.oM.Versioning;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BH.Upgraders
{
    [Upgrader(6, 2)]
    public static class v62
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [VersioningTarget("BH.oM.LadybugTools.SimulationResult")]
        public static Dictionary<string, object> UpgradeSimulationResult(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("GroundMaterial"))
                newVersion["GroundMaterial"] = null;

            if (newVersion.ContainsKey("ShadeMaterial"))
                newVersion["ShadeMaterial"] = null;

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.Lighting.Elements.Luminaire")]
        public static Dictionary<string, object> UpgradeLuminaire(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            if (newVersion.ContainsKey("Direction"))
            {
                if (newVersion["Direction"] == null)
                {
                    newVersion["Orientation"] = null;
                    newVersion.Remove("Direction");
                    return newVersion;
                }

                Dictionary<string, object> basis = null;

                try
                {
                    Dictionary<string, object> xVec = new Dictionary<string, object>() { ["X"] = 1.0, ["Y"] = 0.0, ["Z"] = 0.0 };
                    Dictionary<string, object> yVec = new Dictionary<string, object>() { ["X"] = 0.0, ["Y"] = 1.0, ["Z"] = 0.0 };
                    Dictionary<string, object> zVec = new Dictionary<string, object>() { ["X"] = 0.0, ["Y"] = 0.0, ["Z"] = 1.0 };

                    basis = new Dictionary<string, object>() { ["X"] = xVec, ["Y"] = yVec, ["Z"] = zVec };

                    Dictionary<string, object> orientation = Normalise(newVersion["Direction"] as Dictionary<string, object>);
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
                            basis["X"] = new Dictionary<string, object>() { ["X"] = -1.0, ["Y"] = 0.0, ["Z"] = 0.0 };
                            basis["Y"] = yVec;
                            basis["Z"] = new Dictionary<string, object>() { ["X"] = 0.0, ["Y"] = 0.0, ["Z"] = -1.0 };
                        }
                    }
                    else
                    {
                        basis["X"] = Normalise(CrossProduct(orientation, zVec));
                        basis["Y"] = Normalise(CrossProduct(orientation, xVec));
                        basis["Z"] = orientation;
                    }
                }
                catch { }

                if (basis != null)
                {
                    foreach (object o in basis.Values)
                    {
                        if (o is Dictionary<string, object> vector)
                            vector["_t"] = "BH.oM.Geometry.Vector";
                    }
                    basis["_t"] = "BH.oM.Geometry.Basis";
                }

                newVersion["Orientation"] = basis;
                newVersion.Remove("Direction");
            }
            return newVersion;
        }

        /***************************************************/

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

        /***************************************************/

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

        /***************************************************/

        [VersioningTarget("BH.oM.Base.Attributes.InputAttribute")]
        public static Dictionary<string, object> UpgradeInputAttribute(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            newVersion.Add("Exposure", "Display");

            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.DeepLearning.Models.Graph")]
        public static Dictionary<string, object> UpgradeDeepLearningGraph(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("Modules"))
            {
                newVersion["Modules"] = UpgradeGraph(newVersion["Modules"] as Dictionary<string, object>, "BH.oM.DeepLearning.IModule");
            }
            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.Forms.InputTree`1[[System.Object]]")]
        public static Dictionary<string, object> UpgradeInputTree(Dictionary<string, object> oldVersion)
        {
            return UpgradeGraph(oldVersion, "System.Object");
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeGraph(Dictionary<string, object> oldVersion, string typeRestriction)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (!newVersion.ContainsKey("_t"))
                newVersion["_t"] = $"BH.oM.Data.Collections.Tree`1[[{typeRestriction}]]";


            if (newVersion.ContainsKey("Children"))
            {
                Dictionary<string, object> children = newVersion["Children"] as Dictionary<string, object>;
                if (children != null)
                {
                    Dictionary<string, object> newChildren = new Dictionary<string, object>();
                    foreach (var item in children)
                    {
                        newChildren[item.Key] = UpgradeGraph(item.Value as Dictionary<string, object>, typeRestriction);
                    }
                    newVersion["Children"] = newChildren;
                }

            }
            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.Data.Collections.PointMatrix`1[[System.Object]]")]
        public static Dictionary<string, object> UpgradePointMatrix(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion.ContainsKey("Data"))
            {
                object[] data = newVersion["Data"] as object[];
                if (data != null)
                {
                    object[] newData = new object[data.Length];

                    for (int i = 0; i < data.Length; i++)
                    {
                        Dictionary<string, object> item = data[i] as Dictionary<string, object>;
                        if (item != null)
                        {
                            object[] localData = item["v"] as object[];

                            if (localData != null)
                            {
                                object[] newLocData = new object[localData.Length];
                                for (int j = 0; j < localData.Length; j++)
                                {
                                    if (localData[j] is Dictionary<string, object> dic)
                                    {
                                        dic["_t"] = "BH.oM.Data.Collections.LocalData`1[[System.Object]]";
                                        newLocData[j] = dic;
                                    }
                                }
                                item["v"] = newLocData;
                            }
                            newData[i] = item;
                        }
                    }

                    newVersion["Data"] = newData;
                }

            }
            return newVersion;
        }

        /***************************************************/

        [VersioningTarget("BH.oM.Structure.FloorSystem.FloorDesign")]
        public static Dictionary<string, object> UpgradeFloorDesing(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (!newVersion.ContainsKey("ColumnConfiguration"))
                newVersion["ColumnConfiguration"] = null;

            if (newVersion.ContainsKey("FloorConfiguration"))
                newVersion["FloorConfiguration"] = UpgradeFloorConfiguration(newVersion["FloorConfiguration"] as Dictionary<string, object>);

            Dictionary<string, object> customData;
            if (newVersion.ContainsKey("CustomData"))
                customData = newVersion["CustomData"] as Dictionary<string, object>;
            else
                customData = new Dictionary<string, object>();

            MoveToCustomData(newVersion, customData, "Utilisation");
            MoveToCustomData(newVersion, customData, "MinBaysX");
            MoveToCustomData(newVersion, customData, "MinBaysY");
            MoveToCustomData(newVersion, customData, "GlobalWarmingPotential");
            MoveToCustomData(newVersion, customData, "LifeCycleAssessmentNotes");

            newVersion["CustomData"] = customData;

            return newVersion;
        }

        /***************************************************/

        private static void MoveToCustomData(Dictionary<string, object> newVersion, Dictionary<string, object> customData, string prop)
        {
            if (newVersion.ContainsKey(prop))
            {
                if (newVersion[prop] != null)
                    customData[prop] = newVersion[prop];

                newVersion.Remove(prop);
            }
        }

        /***************************************************/

        private static Dictionary<string, object> UpgradeFloorConfiguration(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);

            if (newVersion["_t"].ToString() == "BH.oM.Structure.FloorSystem.SteelInfillBeams" ||
                newVersion["_t"].ToString() == "BH.oM.Structure.FloorSystem.TimberInfillBeams" ||
                newVersion["_t"].ToString() == "BH.oM.Structure.FloorSystem.CompositeSteelInfillBeams")
            {

                bool primSet = newVersion.ContainsKey("PrimaryBeamTopLevel");
                bool secSet = newVersion.ContainsKey("SecondaryBeamTopLevel");

                if (!primSet || !secSet)
                {
                    //Sets the beam top level to the thickness of the slab as that was previously assumed, but now needs to be set explicitly
                    double thickness = TotalThickness(newVersion["Slab"] as Dictionary<string, object>);

                    if (!primSet)
                        newVersion["PrimaryBeamTopLevel"] = thickness;

                    if (!secSet)
                        newVersion["SecondaryBeamTopLevel"] = thickness;
                }
            }

            if (newVersion["_t"].ToString() == "BH.oM.Structure.FloorSystem.RCFlatPlate")
            {
                if (newVersion.ContainsKey("Reinforcement"))
                {
                    Dictionary<string, object> slabReinforcement = new Dictionary<string, object>();
                    slabReinforcement["_t"] = "BH.oM.Structure.FloorSystem.LayoutSlabReinforcement";
                    slabReinforcement["Reinforcement"] = newVersion["Reinforcement"];
                    newVersion["SlabReinforcement"] = slabReinforcement;
                    newVersion.Remove("Reinforcement");
                }
            }


            return newVersion;
        }

        /***************************************************/

        private static double TotalThickness(Dictionary<string, object> slab)
        {
            try
            {
                if (slab["_t"].ToString() == "BH.oM.Structure.SurfaceProperties.ConstantThickness")
                {
                    return (double)slab["Thickness"];
                }
                else if (slab["_t"].ToString() == "BH.oM.Structure.SurfaceProperties.SlabOnDeck")
                {
                    return (double)slab["DeckHeight"] + (double)slab["SlabThickness"];
                }
                else if (slab["_t"].ToString() == "BH.oM.Structure.SurfaceProperties.Layered")
                {
                    double thickness = 0;
                    if (slab.ContainsKey("Layers"))
                    {
                        object[] layers = slab["Layers"] as object[];
                        foreach (object layer in layers)
                        {
                            Dictionary<string, object> lay = layer as Dictionary<string, object>;
                            if (lay != null && lay.ContainsKey("Thickness"))
                            {
                                thickness += (double)lay["Thickness"];
                            }
                        }
                    }
                    return thickness;
                }
            }
            catch (Exception)
            {

                return double.NaN;
            }
            return double.NaN;
        }

        /***************************************************/
    }
}


