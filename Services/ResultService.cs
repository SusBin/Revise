using Revise.Models;
using System.Text.Json;

namespace Revise.Services
{
    public class ResultService
    {
        private readonly string _filePath;

        public ResultService(IWebHostEnvironment environment)
        {
            _filePath = Path.Combine(environment.ContentRootPath, "results.json");
        }

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
            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Result>>(json);
            }
            catch (Exception ex)
            {
                // Log the exception or print it to the console for debugging
                Console.WriteLine($"Error loading results: {ex.Message}");
                return new List<Result>();
            }
        }

        public int GetNextTestId()
        {
            // Read the JSON from the results file
            var json = File.ReadAllText(_filePath);

            // Deserialize the JSON into a list of Result objects
            var results = JsonSerializer.Deserialize<List<Result>>(json);

            // Find the highest existing TestId and return the next one
            int nextTestId = results.Any() ? results.Max(r => r.TestId) + 1 : 1;

            return nextTestId;
        }



    }
}

