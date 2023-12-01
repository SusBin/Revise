using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Models;
using Revise.Services;

namespace Revise.Pages.Topic
{
    public class CreateModel : PageModel
    {
        private readonly QuestionService _questionService;

        public Revise.Models.Course? Course { get; set; }
        [BindProperty]
        public string NewTopicName { get; set; }

        public CreateModel(QuestionService questionService)
        {
            _questionService = questionService;
        }
        public IActionResult OnGet(int id)
        {
            Course = _questionService.GetCourseById(id);
            if (Course == null)
            {
                TempData["ErrorMessage"] = "No Courses were found with that ID.";
                return RedirectToPage("/Error");
            }
            return Page();
        }

        public IActionResult OnPost(int courseId, string newTopicName)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }            
            _questionService.AddNewTopic(courseId, newTopicName);
            return RedirectToPage("/Course/Index", new { id = courseId });
        }

    }
}
