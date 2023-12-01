using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Services;


namespace Revise.Pages.Course
{
    public class IndexModel : PageModel
    {
        private readonly QuestionService _questionService;
        public Dictionary<int, string> Courses { get; set; }

        public IndexModel(QuestionService questionService)
        {
            _questionService = questionService;
        }

        public void OnGet()
        {
            Courses = _questionService.GetCourses();
        }
    }
}
