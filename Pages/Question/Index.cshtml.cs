using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Services;

namespace Revise.Pages.Question
{

    public class IndexModel : PageModel
    {
        private QuestionService _questionService;
        public List<string> Topics { get; set; }

        public IndexModel(QuestionService questionService)
        {
            _questionService = questionService;
        }

        public void OnGet()
        {
            Topics = _questionService.GetUniqueTopics();
        }
    }
}
