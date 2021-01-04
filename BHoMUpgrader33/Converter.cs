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

namespace BH.Upgrader.v33
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "3.2";

            ToNewObject.Add("BH.oM.Structure.FramingProperties.ConstantFramingElementProperty", UpgradeConstantFramingElementProperty);
            ToNewObject.Add("BH.oM.Structure.Elements.FramingElement", UpgradeFramingElement);

        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        public static Dictionary<string, object> UpgradeConstantFramingElementProperty(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>();

            newVersion["_t"] = "BH.oM.Physical.FramingProperties.ConstantFramingProperty";
            newVersion["OrientationAngle"] = oldVersion["OrientationAngle"];
            newVersion["Name"] = oldVersion["Name"];
            newVersion["CustomData"] = oldVersion["CustomData"];
            newVersion["BHoM_Guid"] = oldVersion["BHoM_Guid"];
            newVersion["Tags"] = oldVersion["Tags"];
            newVersion["Fragments"] = oldVersion["Fragments"];

            object sectionProp;
            if (oldVersion.TryGetValue("SectionProperty", out sectionProp))
            {
                Dictionary<string, object> sectionDict = sectionProp as Dictionary<string, object>;
                object profile;
                if (sectionDict != null && sectionDict.TryGetValue("SectionProfile", out profile))
                {
                    newVersion["Profile"] = profile;
                }

                object strMaterial;
                if (sectionDict != null && sectionDict.TryGetValue("Material", out strMaterial))
                {
                    Dictionary<string, object> strMatDict = strMaterial as Dictionary<string, object>;

                    if (strMatDict != null)
                    {
                        Dictionary<string, object> material = new Dictionary<string, object>();
                        material["_t"] = "BH.oM.Physical.Materials.Material";
                        if (strMatDict.ContainsKey("Name"))
                            material["Name"] = strMatDict["Name"];
                        material["Properties"] = new List<object> { strMatDict };

                        newVersion["Material"] = material;
                    }
                }
            }

            return newVersion;
        }

        /***************************************************/

        public static Dictionary<string, object> UpgradeFramingElement(Dictionary<string, object> oldVersion)
        {
            if (oldVersion == null)
                return null;

            Dictionary<string, object> newVersion = new Dictionary<string, object>();

            string type = "Beam";
            object typeObject;
            if (oldVersion.TryGetValue("StructuralUsage", out typeObject))
                type = typeObject.ToString();

            if(type == "Column")
                newVersion["_t"] = "BH.oM.Physical.Elements.Column";
            else if(type == "Brace")
                newVersion["_t"] = "BH.oM.Physical.Elements.Bracing";
            else if (type == "Cable")
                newVersion["_t"] = "BH.oM.Physical.Elements.Cable";
            else if (type == "Pile")
                newVersion["_t"] = "BH.oM.Physical.Elements.Pile";
            else
                newVersion["_t"] = "BH.oM.Physical.Elements.Beam";



            newVersion["Name"] = oldVersion["Name"];
            newVersion["CustomData"] = oldVersion["CustomData"];
            newVersion["BHoM_Guid"] = oldVersion["BHoM_Guid"];
            newVersion["Tags"] = oldVersion["Tags"];
            newVersion["Fragments"] = oldVersion["Fragments"];
            newVersion["Location"] = oldVersion["LocationCurve"];
            newVersion["Property"] = oldVersion["Property"];

            return newVersion;
        }

        /***************************************************/

    }
}

