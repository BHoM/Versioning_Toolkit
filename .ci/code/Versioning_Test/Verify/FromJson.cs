/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2022, the respective contributors. All rights reserved.
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


using BH.Engine.Base;
using BH.Engine.Serialiser;
using BH.Engine.Test;
using BH.oM.Base;
using BH.oM.Base.Debugging;
using BH.oM.Test;
using BH.oM.Test.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BH.Test.Versioning
{
    public static partial class Verify
    {
        /*************************************/
        /**** Test Methods                ****/
        /*************************************/

        public static TestResult FromJsonDatasets(bool testAll = false)
        {
            string testFolder = @"C:\ProgramData\BHoM\Datasets\TestSets\Versioning";
            List<string> versions = new List<string> { "5.3", };
            if(testAll)
                versions.AddRange(new List<string> { "5.2", "5.1", "5.0", "4.3", "4.2", "4.1", "4.0", "3.3" });

            string exceptions = "Grasshopper|Rhinoceros";

            // Test all the BHoM versions available
            List<TestResult> results = versions.Select(v => Path.Combine(testFolder, v)).Select(x => FromJsonDataset(x, exceptions)).ToList();

            // Generate the result message
            int errorCount = results.Where(x => x.Status == TestStatus.Error).Count();
            int warningCount = results.Where(x => x.Status == TestStatus.Warning).Count();

            // Returns a summary result 
            return new TestResult()
            {
                ID = "VersioningFromJsonDatasets",
                Description = $"Versioning from json datasets generated with {results.Count} previous beta versions of the BHoM.",
                Message = $"{errorCount} errors and {warningCount} warnings reported.",
                Status = results.MostSevereStatus(),
                Information = results.Where(x => x.Status != TestStatus.Pass).ToList<ITestInformation>(),
                UTCTime = DateTime.UtcNow,
            };
        }

        /*************************************/

        public static TestResult FromJsonDataset(string testFolder, string exceptions = "")
        {
            Engine.Base.Compute.LoadAllAssemblies();
            List<TestResult> results = new List<TestResult>();

            // Test all objects
            int nbObjects = 0;
            string objectFile = Path.Combine(testFolder, "Objects.json");
            if (File.Exists(objectFile))
            {
                IEnumerable<string> json = File.ReadAllLines(objectFile).Where(x => !string.IsNullOrWhiteSpace(x) && (exceptions.Length == 0 || !Regex.IsMatch(x, exceptions)));
                results.AddRange(json.Select(x => FromJsonItem(x, false)));

                nbObjects = json.Count();
            }

            // Test all methods
            int nbMethods = 0;
            string methodFile = Path.Combine(testFolder, "Methods.json");
            if (File.Exists(methodFile))
            {
                IEnumerable<string> json = File.ReadAllLines(methodFile).Where(x => !string.IsNullOrWhiteSpace(x) && (exceptions.Length == 0 || !Regex.IsMatch(x, exceptions)));
                results.AddRange(json.Select(x => FromJsonItem(x, true)));

                nbMethods = json.Count();
            }

            // Returns a summary result 
            string version = Path.GetFileName(testFolder);
            int errorCount = results.Where(x => x.Status == TestStatus.Error).Count();
            int warningCount = results.Where(x => x.Status == TestStatus.Warning).Count();

            return new TestResult()
            {
                ID = $"VersioningFromJsonDatasets_{version}",
                Description = $"Beta Version {Path.GetFileName(testFolder)}: {nbObjects} object types and {nbMethods} methods.",
                Message = $"{errorCount} errors and {warningCount} warnings reported.",
                Status = results.MostSevereStatus(),
                Information = results.Where(x => x.Status != TestStatus.Pass).ToList<ITestInformation>(),
                UTCTime = DateTime.UtcNow,
            };
        }

        /*************************************/

        public static TestResult FromJsonItem(string json, bool isMethod)
        {
            object result = null;
            bool detected = false;
            try
            {
                Engine.Base.Compute.ClearCurrentEvents();
                result = Engine.Serialiser.Convert.FromJson(json);
                detected = BH.Engine.Base.Query.CurrentEvents().Any(x => x.Message.StartsWith("No upgrade for"));
            }
            catch (Exception e)
            {
                Engine.Base.Compute.RecordError("Deserialisation from json failed. Error: " + e.Message);
            }

            bool success = result != null || detected;
            if (!success && isMethod)
                success = Helpers.CanReplaceMethodWithType(json);

            string description = "";
            if (result != null && !(result is CustomObject))
                description = result.IToText(true);
            else
                description = Helpers.DescriptionFromJson(json);

            string message = "";
            if (!success)
                message = $"Error: Returned null from json.";
            else if (!detected && result is CustomObject)
                message = $"Error: Result returned as CustomObject";

            if (message == "")
                return Engine.Test.Create.PassResult(description);
            else
                return new TestResult
                {
                    Description = description,
                    Status = TestStatus.Error,
                    Message = message,
                    Information = Engine.Base.Query.CurrentEvents().Select(x => x.ToEventMessage()).ToList<ITestInformation>()
                };
        }

        /*************************************/
    }
}


