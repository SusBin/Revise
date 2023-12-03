using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Models;
using Revise.Services;

namespace Revise.Pages.Question
{
    public class CreateModel : PageModel
    {
        private readonly QuestionService _questionService;
        [BindProperty]
        public RevisionQuestion NewQuestion { get; set; }

        [BindProperty(SupportsGet = true)]
        public int CourseId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string TopicName { get; set; }
        public CreateModel(QuestionService questionService)
        {
            _questionService = questionService;
        }
        public void OnGet(int id, string topic)
        {
            CourseId = id;
            TopicName = topic;
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                var errorMessages = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        errorMessages.Add(error.ErrorMessage);
                    }
                }
                TempData["ErrorMessage"] = string.Join(", ", errorMessages);
                return RedirectToPage("/Error");
            }

            try
            {
                // Add the new question to your data source here
                _questionService.AddNewQuestion(CourseId, TopicName, NewQuestion);

                return RedirectToPage("./View", new { topic = TopicName });

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Error");
            }
        }
    }
}
