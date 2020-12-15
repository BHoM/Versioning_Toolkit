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
                            List<string> list = custom.CustomData["Objects"] as List<string>;
                            if (list != null)
                                objectsToIgnore = new HashSet<string>(list);
                        }
                        if (custom.CustomData.ContainsKey("Methods"))
                        {
                            List<string> list = custom.CustomData["Methods"] as List<string>;
                            if (methodsToIgnore == null)
                                methodsToIgnore = new HashSet<string>(list);
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
                    .Select(x => FromJsonItem(x))
                    .Where(x => x.Status == ResultStatus.Fail && !objectsToIgnore.Contains(x.Description)));
            }

            // Test all methods
            string methodFile = Path.Combine(testFolder, "Methods.json");
            if (File.Exists(methodFile))
            {
                fails.AddRange(File.ReadAllLines(methodFile)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => FromJsonItem(x))
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

        public static TestResult FromJsonItem(string json)
        {
            object result = Engine.Serialiser.Convert.FromJson(json);

            string description = "";
            if (result != null && !(result is CustomObject))
                description = result.IToText(true);
            else
                description = Helpers.DescriptionFromJson(json);

            if (result == null)
                return Engine.Test.Create.FailResult($"Failed to recover {description}", description, false);
            else if (result is CustomObject)
                return Engine.Test.Create.FailResult($"Object returned as CustomObject: {description}", description, false);
            else
                return Engine.Test.Create.PassResult(description);
        }

        /*************************************/
    }
}
