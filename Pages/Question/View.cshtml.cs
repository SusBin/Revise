using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Models;
using Revise.Services;

namespace Revise.Pages.Question
{
    public class QuestionModel : PageModel
    {
        private QuestionService _questionService;

        public QuestionModel(QuestionService questionService)
        {
            _questionService = questionService;
        }

        public List<RevisionQuestion> Questions { get; set; }
        public int CourseId { get; set; }
        public void OnGet(string topic)
        {
            CourseId = _questionService.GetCourseIdbyTopic(topic);
            Questions = _questionService.GetQuestionsByTopic(topic);
        }
    }
}
