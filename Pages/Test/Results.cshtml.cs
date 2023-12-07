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
            _resultService = resultService;
        }

        public void OnGet()
        {
            // Load the results when the page is accessed
            Results = _resultService.LoadResults();
        }
    }
}
