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

namespace BH.Upgrader.v50
{
    public class Converter : Base.Converter
    {
        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public Converter() : base()
        {
            PreviousVersion = "4.3";

            ToNewObject.Add("BH.oM.Revit.PinnedButtonInfo", UpgradePinnedButtonInfo);
        }


        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/

        private static Dictionary<string, object> UpgradePinnedButtonInfo(Dictionary<string, object> oldVersion)
        {
            Dictionary<string, object> newVersion = new Dictionary<string, object>(oldVersion);
            newVersion["_t"] = "BH.oM.Revit.PinnedItemInfo";

            newVersion["Method"] = "[]";
            if (newVersion.ContainsKey("InfoString"))
            {
                string infoString = newVersion["InfoString"] as string;
                string typeName, methodName;
                List<string> parameters;
                if (ParseInfoString(infoString, out typeName, out methodName, out parameters))
                {
                    Dictionary<string, object> methodProps = new Dictionary<string, object>();
                    methodProps["_t"] = "System.Reflection.MethodBase";
                    methodProps["TypeName"] = $"{{ \"_t\" : \"System.Type\", \"Name\" : \"{typeName}\", \"_bhomVersion\" : \"5.0\" }}";
                    methodProps["MethodName"] = methodName;
                    methodProps["Parameters"] = parameters.Select(x => ParameterString(x)).ToList();
                    methodProps["_bhomVersion"] = "5.0";
                    newVersion["Method"] = methodProps;
                }                    
                
                newVersion.Remove("InfoString");
            }

            return newVersion;
        }

        /***************************************************/

        private static bool ParseInfoString(string infostring, out string typeName, out string methodName, out List<string> parameters)
        {
            typeName = "";
            methodName = "";
            parameters = null;
            if (string.IsNullOrWhiteSpace(infostring))
                return false;

            int parenthesis = infostring.IndexOf('(');
            string method = infostring.Substring(0, parenthesis);
            int dot = method.LastIndexOf('.');
            typeName = method.Substring(0, dot);
            methodName = method.Substring(dot + 1);
            parameters = infostring.Substring(parenthesis + 1, infostring.Length - parenthesis - 2).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Replace(" ", "")).ToList();

            return true;
        }

        /***************************************************/

        private static string ParameterString(string param)
        {
            param = param.Trim();
            if (!param.Contains("."))
                param = $"System.{param}";

            if (param.EndsWith("]") && !param.EndsWith("[]"))
            {
                int bracket = param.IndexOf('[');
                string genericArgs = param.Substring(bracket + 1, param.Length - bracket - 2);
                genericArgs = string.Join(", ", genericArgs.Split(',').Select(x => ParameterString(x)));

                return $"{{ \"_t\" : \"System.Type\", \"Name\" : \"{param.Substring(0, bracket)}\", \"GenericArguments\" : [{genericArgs}], \"_bhomVersion\" : \"5.0\" }}";
            }
            else
                return $"{{ \"_t\" : \"System.Type\", \"Name\" : \"{param}\", \"_bhomVersion\" : \"5.0\" }}";
        }

        /***************************************************/
    }
}