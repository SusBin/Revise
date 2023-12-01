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
        public List<string> Topics { get; set; }
        [BindProperty]
        public Revise.Models.Course Course { get; set; }
        [BindProperty]
        public RevisionQuestion RevisionQuestion { get; set; }
        [BindProperty]
        public string TopicName { get; set; }

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
            Topics = _questionService.GetTopicsByCourseId(id);
            return Page();
        }

        public IActionResult OnPost(int courseId, string topicName)
        {
            try
            {
                var courses = new List<Revise.Models.Course>();
                if (System.IO.File.Exists("questions.json"))
                {
                    var json = System.IO.File.ReadAllText("questions.json");
                    courses = JsonSerializer.Deserialize<List<Revise.Models.Course>>(json);
                }

                var course = courses.FirstOrDefault(c => c.Id == courseId);
                if (course == null)
                {
                    throw new Exception("Course not found.");
                }

                var topic = course.Topics.FirstOrDefault(t => t.Topic == TopicName);
                if (topic == null)
                {
                    // If the topic is not found, create a new one
                    topic = new StudyTopic { Topic = TopicName, Questions = new List<RevisionQuestion>() };
                    course.Topics.Add(topic);
                }

                // Auto-generate the Id field
                RevisionQuestion.Id = topic.Questions.Any() ? topic.Questions.Max(q => q.Id) + 1 : 1;

                topic.Questions.Add(RevisionQuestion);
                var newJson = JsonSerializer.Serialize(courses);
                System.IO.File.WriteAllText("questions.json", newJson);

                return RedirectToPage("/Success");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Error");
            }
        }



    }
}
