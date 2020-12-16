/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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


using BH.Engine.Reflection;
using BH.Engine.Serialiser;
using BH.Engine.Test;
using BH.oM.Base;
using BH.oM.Reflection.Debugging;
using BH.oM.Test.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Test.Versioning
{
    public static partial class Verify
    {
        /*************************************/
        /**** Test Methods                ****/
        /*************************************/

        public static TestResult FromJsonDatasets()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string testFolder = Path.Combine(currentDirectory, @"..\Datasets\TestSets\Versioning");

            // Test all the BHoM versions available
            List<TestResult> results = Directory.GetDirectories(testFolder).Select(x => FromJsonDataset(x)).ToList();

            // Returns a summary result 
            List<TestResult> fails = results.Where(x => x.Status == ResultStatus.Fail).ToList();
            foreach (TestResult fail in fails)
            {
                foreach (Event e in fail.Events)
                    e.Message = fail.Description + ": " + e.Message;
            }
            string description = $"{results.Count} versions of the BHoM tested";
            if (fails.Count == 0)
                return Engine.Test.Create.PassResult(description);
            else
                return new TestResult(ResultStatus.Fail, fails.SelectMany(x => x.Events).ToList(), description);
        }

        /*************************************/

        public static TestResult FromJsonDataset(string testFolder)
        {
            Engine.Reflection.Compute.LoadAllAssemblies();

            // Read all exceptions
            string exceptionFile = Path.Combine(testFolder, "Exceptions.json");
            HashSet<string> objectsToIgnore = new HashSet<string>();
            HashSet<string> methodsToIgnore = new HashSet<string>();
            if (File.Exists(exceptionFile))
            {
                try
                {
                    string json = File.ReadAllText(exceptionFile);
                    CustomObject custom = Engine.Serialiser.Convert.FromJson(json) as CustomObject;
                    if (custom != null)
                    {
                        if (custom.CustomData.ContainsKey("Objects"))
                        {
                            List<object> list = custom.CustomData["Objects"] as List<object>;
                            if (list != null)
                                objectsToIgnore = new HashSet<string>(list.OfType<string>());
                        }
                        if (custom.CustomData.ContainsKey("Methods"))
                        {
                            List<object> list = custom.CustomData["Methods"] as List<object>;
                            if (list != null)
                                methodsToIgnore = new HashSet<string>(list.OfType<string>());
                        }
                    }
                }
                catch { }
            }

            // Test all objects
            List<TestResult> fails = new List<TestResult>();
            string objectFile = Path.Combine(testFolder, "Objects.json");
            if (File.Exists(objectFile))
            {
                fails.AddRange(File.ReadAllLines(objectFile)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => FromJsonItem(x, false))
                    .Where(x => x.Status == ResultStatus.Fail && !objectsToIgnore.Contains(x.Description)));
            }

            // Test all methods
            string methodFile = Path.Combine(testFolder, "Methods.json");
            if (File.Exists(methodFile))
            {
                fails.AddRange(File.ReadAllLines(methodFile)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => FromJsonItem(x, true))
                    .Where(x => x.Status == ResultStatus.Fail && !methodsToIgnore.Contains(x.Description)));
            }

            // Returns a summary result 
            string description = $"Version {Path.GetFileName(testFolder)}";
            if (fails.Count == 0)
                return Engine.Test.Create.PassResult(description);
            else
                return new TestResult(ResultStatus.Fail, fails.SelectMany(x => x.Events).ToList(), description);
        }

        /*************************************/

        public static TestResult FromJsonItem(string json, bool isMethod)
        {
            
            object result = Engine.Serialiser.Convert.FromJson(json);
            bool success = result != null;
            if (!success && isMethod)
                success = Helpers.CanReplaceMethodWithType(json);

            string description = "";
            if (result != null && !(result is CustomObject))
                description = result.IToText(true);
            else
                description = Helpers.DescriptionFromJson(json);

            if (!success)
                return Engine.Test.Create.FailResult($"Failed to recover {description}", description, false);
            else if (result is CustomObject)
                return Engine.Test.Create.FailResult($"Object returned as CustomObject: {description}", description, false);
            else
                return Engine.Test.Create.PassResult(description);
        }

        /*************************************/
    }
}
