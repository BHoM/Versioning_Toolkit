// See https://aka.ms/new-console-template for more information
using BH.oM.Test.Results;

TestResult result = BH.Test.Versioning.Verify.FromJsonDatasets();

Console.WriteLine("Result: " + result.Status);
Console.WriteLine(result.Message);
