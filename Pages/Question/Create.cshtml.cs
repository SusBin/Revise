using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Models;
using Revise.Services;
using System.Text.Json;

namespace Revise.Pages.Question
{
    public class CreateModel : PageModel
    {
        private readonly QuestionService _questionService;
        [BindProperty]
        public StudyTopic Topic { get; set; }
        [BindProperty]
        public Revise.Models.Course Course { get; set; }
        [BindProperty]
        public List<string> Topics { get; private set; }
        [BindProperty]
        public string CourseCode { get; private set; }
        [BindProperty]
        public string CourseName { get; private set; }
                
        public CreateModel(QuestionService questionService)
        {
            _questionService = questionService;
        }

        public IActionResult OnGet(int id, string topic)
        {
            Course = _questionService.GetCourseById(id);
            Topics = _questionService.GetTopicsByCourseId(id);
            CourseCode = Course.CourseCode;
            CourseName = Course.CourseName;
            if (Course == null)
            {
                TempData["ErrorMessage"] = "No Courses were found with that ID.";
                return RedirectToPage("/Error");
            }
            Topic = _questionService.GetStudyTopic(id, topic);
            return Page();
        }

        public IActionResult OnPost(int courseId)
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
                var courses = new List<Revise.Models.Course>();
                if (System.IO.File.Exists("questions.json"))
                {
                    var json = System.IO.File.ReadAllText("questions.json");
                    courses = JsonSerializer.Deserialize<List<Revise.Models.Course>>(json);
                }

                var course = _questionService.GetCourseById(courseId) ?? throw new Exception("Course not found.");

                // Create a new question
                var newQuestion = new RevisionQuestion();

                // Auto-generate the Id field
                newQuestion.Id = Topic.Questions.Any() ? Topic.Questions.Max(q => q.Id) + 1 : 1;

                // Add the new question to the Questions list
                Topic.Questions.Add(newQuestion);

                var newJson = JsonSerializer.Serialize(courses);
                System.IO.File.WriteAllText("questions.json", newJson);

                return RedirectToPage("/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Error");
            }
        }



    }
}
