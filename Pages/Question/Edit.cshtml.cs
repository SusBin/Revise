using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Models;
using Revise.Services;

namespace Revise.Pages.Question
{
    public class EditModel : PageModel
    {
        private QuestionService _questionService;
        [BindProperty]
        public RevisionQuestion Question { get; set; }
        [BindProperty]
        public string TopicName { get; set; }

        public EditModel(QuestionService questionService)
        {
            _questionService = questionService;
        }

        public void OnGet(int courseId, string topicName, int id)
        {
            Question = _questionService.GetQuestionById(courseId, topicName, id);
            TopicName = topicName;
        }

        public IActionResult OnPost(int courseId, string topicName)
        {
            if (!ModelState.IsValid)
            {
                return Page(); // Return the same page to display validation errors
            }
            // If the checkbox was not checked, set IsCorrect to false
            foreach (var answer in Question.Answers)
            {
                if (answer.IsCorrect == null)
                {
                    answer.IsCorrect = false;
                }
            }

            _questionService.UpdateQuestion(courseId, topicName, Question);

            return RedirectToPage("./Index"); // Redirect to another page after successful update
        }

    }
}
