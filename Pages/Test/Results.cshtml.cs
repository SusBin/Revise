using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Models;
using Revise.Services;

namespace Revise.Pages.Test
{
    public class ResultsModel : PageModel
    {
        private readonly ResultService _resultService;
        public List<Result> Results { get; set; }

        public ResultsModel(ResultService resultService)
        {
            _resultService = resultService ?? throw new ArgumentNullException(nameof(resultService));
        }

        public void OnGet()
        {
            try
            {
                // Load the results when the page is accessed
                Results = _resultService.LoadResults();
                // Add a debug statement
                Console.WriteLine("Results loaded successfully.");
            }
            catch (Exception ex)
            {
                // Handle the exception (log, display an error message, etc.)
                Console.WriteLine($"Error loading results: {ex.Message}");
                Results = new List<Result>();
            }
        }


    }
}
