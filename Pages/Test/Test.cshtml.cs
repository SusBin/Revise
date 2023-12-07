using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Models;
using Revise.Services;

namespace Revise.Pages.Test
{
    public class TestModel : PageModel
    {
        private readonly QuestionService _questionService;
        private readonly ResultService _resultService;
        public List<RevisionQuestion> Questions { get; set; }

        public TestModel(QuestionService questionService, ResultService resultService)
        {
            _questionService = questionService;
            _resultService = resultService;
        }

        public void OnGet(int courseId, int numQuestions)
        {
            Questions = new List<RevisionQuestion>();
            for (int i = 0; i < numQuestions; i++)
            {
                var question = _questionService.GetRandomQuestion(courseId);
                Questions.Add(question);
            }
        }

        public IActionResult OnPost(int courseId, int numQuestions, Dictionary<int, List<int>> answers)
        {
            // Repopulate Questions
            Questions = new List<RevisionQuestion>();
            for (int i = 0; i < numQuestions; i++)
            {
                var question = _questionService.GetRandomQuestion(courseId);
                Questions.Add(question);
            }

            int correctAnswers = 0;

            foreach (var question in Questions)
            {
                if (answers.TryGetValue(question.Id, out var selectedAnswerIndices))
                {
                    // Get the indices of the correct answers
                    var correctAnswerIndices = new List<int>();
                    for (int i = 0; i < question.Answers.Count; i++)
                    {
                        if (question.Answers[i].IsCorrect)
                        {
                            correctAnswerIndices.Add(i);
                        }
                    }

                    // Check if all the correct answers and only the correct answers were selected
                    if (selectedAnswerIndices.OrderBy(a => a).SequenceEqual(correctAnswerIndices.OrderBy(a => a)))
                    {
                        correctAnswers++;
                    }
                }
            }

            // Calculate the score as a percentage
            double scorePercentage = (double)correctAnswers / Questions.Count * 100;

            // Create a new result and save it
            var result = new Result
            {
                TestId = 1, // Replace with your actual TestId
                Date = DateTime.Now,
                ScorePercentage = scorePercentage
            };
            _resultService.SaveResult(result);

            // Redirect the user to a results page
            return RedirectToPage("./Results");
        }


    }
}
