using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Models;
using Revise.Services;
using System.Collections.Generic;

namespace Revise.Pages.Test
{
    public class ResultsModel : PageModel
    {
        private readonly ResultService _resultService;
        private readonly QuestionService _questionService;
        public List<Result> Results { get; set; }
        public Dictionary<int, List<RevisionQuestion>> IncorrectQuestionsPerTest { get; set; }

        public ResultsModel(ResultService resultService, QuestionService questionService)
        {
            _resultService = resultService ?? throw new ArgumentNullException(nameof(resultService));
            _questionService = questionService ?? throw new ArgumentNullException(nameof(questionService));
            IncorrectQuestionsPerTest = new Dictionary<int, List<RevisionQuestion>>();
        }

        public void OnGet()
        {
            try
            {
                // Load the results when the page is accessed
                Results = _resultService.LoadResults();
                foreach (var result in Results)
                {
                    if (result.IncorrectQuestions.Count > 0)
                    {
                        var incorrectQuestionsForThisTest = new List<RevisionQuestion>();
                        foreach (var questionId in result.IncorrectQuestions.Keys)
                        {
                            var question = _questionService.GetQuestionById(questionId);
                            if (question != null)
                            {
                                incorrectQuestionsForThisTest.Add(question);
                            }
                        }
                        IncorrectQuestionsPerTest.Add(result.TestId, incorrectQuestionsForThisTest);
                    }
                }
                // Add a debug statement
                Console.WriteLine("Results loaded successfully.");
            }
            catch (Exception ex)
            {
                // Handle the exception (log, display an error message, etc.)
                Console.WriteLine($"Error loading results: {ex.Message}");
                Results = new List<Result>();
            }
        }
    }
}
