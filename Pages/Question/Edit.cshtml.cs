using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Models;
using Revise.Services;

namespace Revise.Pages.Question
{
    public class EditModel : PageModel
    {
        private QuestionService _questionService;
        public RevisionQuestion Question { get; set; }

        public EditModel(QuestionService questionService)
        {
            _questionService = questionService;
        }

        public void OnGet(int id)
        {
            Question = _questionService.GetQuestionById(id);
        }

        public void OnPost()
        {
            _questionService.UpdateQuestion(Question);
        }
    }
}
