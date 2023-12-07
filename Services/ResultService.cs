using Revise.Models;
using System.Text.Json;

namespace Revise.Services
{
    public class ResultService
    {
        public void SaveResult(Result result)
        {
            // Load the existing results
            var results = LoadResults();

            // Add the new result to the list
            results.Add(result);

            // Convert the list to JSON
            var json = JsonSerializer.Serialize(results);

            // Write the JSON to the results file
            File.WriteAllText("results.json", json);
        }


        public List<Result> LoadResults()
        {
            // Read the JSON from the results file
            var json = File.ReadAllText("results.json");

            // Define a helper class to match the structure of the JSON data
            var helper = new { Results = new List<Result>() };

            // Deserialize the JSON data into the helper object
            var data = JsonSerializer.Deserialize(json, helper.GetType());

            // Extract the list of results from the helper object
            var results = ((List<Result>)data.GetType().GetProperty("Results").GetValue(data, null));

            return results;
        }


    }
}

