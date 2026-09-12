/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2026, the respective contributors. All rights reserved.
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


using System.Text;

namespace BH.Test.Versioning
{
    public static partial class Helpers
    {
        /*************************************/
        /**** Public Methods              ****/
        /*************************************/

        public static string TopLevelFieldFromJson(string json, string field)
        {
            if (string.IsNullOrEmpty(json) || string.IsNullOrEmpty(field))
                return null;

            int depth = 0;
            bool inString = false;
            bool escaped = false;
            int stringStart = -1;

            for (int i = 0; i < json.Length; i++)
            {
                char c = json[i];

                if (inString)
                {
                    if (escaped)
                        escaped = false;
                    else if (c == '\\')
                        escaped = true;
                    else if (c == '"')
                    {
                        inString = false;

                        // A string is a key only when the next non-blank character is a colon; a
                        // value never is. Depth 1 is the record's own object, so this ignores an
                        // identically named field on a nested fragment.
                        bool isWantedName = i - stringStart - 1 == field.Length
                            && string.CompareOrdinal(json, stringStart + 1, field, 0, field.Length) == 0;

                        if (depth == 1 && isWantedName)
                        {
                            int j = i + 1;
                            while (j < json.Length && (json[j] == ' ' || json[j] == '\t'))
                                j++;

                            if (j < json.Length && json[j] == ':')
                                return StringValueAt(json, j + 1);
                        }
                    }

                    continue;
                }

                if (c == '"')
                {
                    inString = true;
                    escaped = false;
                    stringStart = i;
                }
                else if (c == '{' || c == '[')
                    depth++;
                else if (c == '}' || c == ']')
                    depth--;
            }

            return null;
        }

        /*************************************/
        /**** Private Methods             ****/
        /*************************************/

        private static string StringValueAt(string json, int from)
        {
            int i = from;
            while (i < json.Length && (json[i] == ' ' || json[i] == '\t'))
                i++;

            // The field exists but does not hold a string. Returning null rather than guessing
            // keeps a malformed record on the existing namespace path instead of inventing a
            // declaring assembly for it.
            if (i >= json.Length || json[i] != '"')
                return null;

            StringBuilder value = new StringBuilder();
            bool escaped = false;

            for (i++; i < json.Length; i++)
            {
                char c = json[i];

                if (escaped)
                {
                    value.Append(c);
                    escaped = false;
                }
                else if (c == '\\')
                    escaped = true;
                else if (c == '"')
                    return value.ToString();
                else
                    value.Append(c);
            }

            return null;
        }

        /*************************************/
    }
}
