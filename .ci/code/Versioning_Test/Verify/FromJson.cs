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


using BH.Engine.Base;
using BH.Engine.Test;
using BH.oM.Base;
using BH.oM.Test;
using BH.oM.Test.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
            List<string> versions = new List<string> { "9.0" };
            if (testAll)
                versions.AddRange(new List<string> { "8.3", "8.2", "8.1", "8.0", "7.3", "7.2", "7.1", "7.0", "6.3", "6.2", "6.1", "6.0", "5.3", "5.2", "5.1", "5.0", "4.3", "4.2", "4.1", "4.0", "3.3" });

            string exceptions = "Grasshopper|Rhinoceros";

            string failureDumpDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(failureDumpDir);
            if (versions.Count == 1)
                Console.WriteLine($"Failures are dumped in {failureDumpDir} directory.");
            else
                Console.WriteLine($"Multiple versions are being tested, failures per each are dumped in a correspondent subfolder of {failureDumpDir} directory.");

            // Test all the BHoM versions available
            List<TestResult> results = new List<TestResult>();
            foreach (string version in versions)
            {
                string dumpDir = failureDumpDir;
                if (!string.IsNullOrWhiteSpace(dumpDir) && versions.Count > 1)
                    dumpDir = Path.Combine(dumpDir, version);

                results.Add(FromJsonDataset(Path.Combine(testFolder, version), exceptions, failureDumpDir));
            }

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

        public static TestResult FromJsonDataset(string testFolder, string exceptions = "", string failureDumpDir = null)
        {
            Engine.Base.Compute.LoadAllAssemblies();
            List<TestResult> results = new List<TestResult>();

            // Test all objects
            List<string> failingObjects = new List<string>();
            int nbObjects = 0;
            string objectFile = Path.Combine(testFolder, "Objects.json");
            if (File.Exists(objectFile))
            {
                IEnumerable<string> jsons = File.ReadAllLines(objectFile).Where(x => !string.IsNullOrWhiteSpace(x) && (exceptions.Length == 0 || !Regex.IsMatch(x, exceptions)));
                foreach (string json in jsons)
                {
                    TestResult result = FromJsonItem(json, false);
                    if (result.Status == TestStatus.Error)
                        failingObjects.Add(json);

                    results.Add(result);
                }

                nbObjects = jsons.Count();
            }

            // Test all methods
            List<string> failingMethods = new List<string>();
            int nbMethods = 0;
            string methodFile = Path.Combine(testFolder, "Methods.json");
            if (File.Exists(methodFile))
            {
                IEnumerable<string> jsons = File.ReadAllLines(methodFile).Where(x => !string.IsNullOrWhiteSpace(x) && (exceptions.Length == 0 || !Regex.IsMatch(x, exceptions)));
                foreach (string json in jsons)
                {
                    TestResult result = FromJsonItem(json, true);
                    if (result.Status == TestStatus.Error)
                        failingMethods.Add(json);

                    results.Add(result);
                }

                nbMethods = jsons.Count();
            }

            //Test all datasets
            List<string> failingDatasets = new List<string>();
            int nbDatasets = 0;
            string datasetsFile = Path.Combine(testFolder, "Datasets.txt");
            if (File.Exists(datasetsFile))
            {
                IEnumerable<string> datasets = File.ReadAllLines(datasetsFile);
                foreach (string dataset in datasets)
                {
                    TestResult result = FromDataset(dataset);
                    if (result.Status == TestStatus.Error)
                        failingDatasets.Add(dataset);

                    results.Add(result);
                }

                nbDatasets = datasets.Count();
            }

            //Test all Adapters
            List<string> failingAdapters = new List<string>();
            int nbAdapters = 0;
            string adaptersFile = Path.Combine(testFolder, "Adapters.json");
            if (File.Exists(adaptersFile))
            {
                IEnumerable<string> adapters = File.ReadAllLines(adaptersFile);
                foreach (string adapter in adapters)
                {
                    TestResult result = FromJsonItem(adapter, false);
                    if (result.Status == TestStatus.Error)
                        failingAdapters.Add(adapter);

                    results.Add(result);
                }

                nbAdapters = adapters.Count();
            }

            // Dump failures
            if (!string.IsNullOrWhiteSpace(failureDumpDir))
            {
                List<(string, List<string>)> failures = new List<(string, List<string>)>
                {
                    ("Objects", failingObjects),
                    ("Methods", failingMethods),
                    ("Datasets", failingDatasets),
                    ("Adapters", failingAdapters)
                }.Where(x => x.Item2.Count != 0).ToList();

                if (failures.Count != 0)
                {
                    try
                    {
                        Directory.CreateDirectory(failureDumpDir);
                        foreach ((string, List<string>) failure in failures)
                        {
                            File.WriteAllLines(Path.Combine(failureDumpDir, $"{failure.Item1}.txt"), failure.Item2);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to dump failures to {failureDumpDir} because of the following error:\n{ex.Message}");
                    }
                }
            }

            // Returns a summary result 
            string version = Path.GetFileName(testFolder);
            int errorCount = results.Where(x => x.Status == TestStatus.Error).Count();
            int warningCount = results.Where(x => x.Status == TestStatus.Warning).Count();

            return new TestResult()
            {
                ID = $"VersioningFromJsonDatasets_{version}",
                Description = $"Beta Version {Path.GetFileName(testFolder)}: {nbObjects} object types, {nbMethods} methods, {nbDatasets} datasets, and {nbAdapters} adapters.",
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

        public static TestResult FromDataset(string dataset)
        {
            List<TestResult> results = new List<TestResult>();

            if (string.IsNullOrEmpty(dataset))
                Engine.Test.Create.PassResult($"No versioning errors for {dataset} found.");

            Engine.Base.Compute.ClearCurrentEvents();
            string result = BH.Engine.Library.Query.ValidatePath(dataset);

            bool failure = BH.Engine.Base.Query.CurrentEvents().Any(x => x.Message.Contains("is not a valid") && x.Message.Contains("no valid upgrade"));

            if (failure)
            {
                return new TestResult()
                {
                    Description = dataset,
                    Status = TestStatus.Error,
                    Message = $"No valid dataset could be found for {dataset}.",
                    Information = Engine.Base.Query.CurrentEvents().Select(x => x.ToEventMessage()).ToList<ITestInformation>(),
                };
            }
            else
                return Engine.Test.Create.PassResult($"No versioning errors for {dataset} found.");
        }

        /*************************************/
    }
}

