using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Revise.Models;
using Revise.Services;
using System;
using System.Reflection;

namespace Revise.Pages.Test
{
    public class TestModel : PageModel
    {
        private readonly QuestionService _questionService;
        private readonly ResultService _resultService;
        public List<RevisionQuestion> Questions { get; set; }
        public Dictionary<int, string> Courses { get; set; }

        public TestModel(QuestionService questionService, ResultService resultService)
        {
            _questionService = questionService;
            _resultService = resultService;
        }

        public void OnGet(int courseId, int numQuestions)
        {
            Courses = _questionService.GetCourses();
            Questions = new List<RevisionQuestion>();
            for (int i = 0; i < numQuestions; i++)
            {
                var question = _questionService.GetRandomQuestion(courseId);
                Questions.Add(question);
            }
        }
        [ValidateAntiForgeryToken]
        public IActionResult OnPost(int courseId, int numQuestions, Dictionary<int, List<int>> answers)
        {
            // Validate inputs and handle errors if necessary

            // Repopulate Questions
            Questions = new List<RevisionQuestion>();

            foreach (var questionId in answers.Keys)
            {
                var question = _questionService.GetQuestionById(questionId);
                if (question != null)
                {
                    Questions.Add(question);
                }
            }
            int correctQuestions = 0;


            foreach (var question in Questions)
            {
                int correctAnswers = 0;
                // Get the indices of the correct answers
                var correctAnswerIndices = question.Answers
                    .Select((answer, index) => new { Answer = answer, Index = index })
                    .Where(item => item.Answer.IsCorrect)
                    .Select(item => item.Index)
                    .ToList();
                var incorrectAnswerIndices = question.Answers
                    .Select((answer, index) => new { Answer = answer, Index = index })
                    .Where(item => !item.Answer.IsCorrect)
                    .Select(item => item.Index)
                    .ToList();

                // Iterate through each correct answer index
                foreach (var index in correctAnswerIndices)
                {
                    // Construct the checkbox name for the current question and index
                    var checkboxName = $"answers[{question.Id}][{index}]";

                    // Check if the checkbox for the current question and index was checked
                    if (Request.Form[checkboxName].Count > 0)
                    {
                        // The checkbox was checked, and the value is "true"                                                
                        // Increment the correctAnswers count,
                        correctAnswers++;
                    }
                }
                //check for incorrect answers
                foreach (var index in incorrectAnswerIndices)
                {
                    // Construct the checkbox name for the current question and index
                    var checkboxName = $"answers[{question.Id}][{index}]";

                    // Check if the checkbox for the current question and index was checked
                    if (Request.Form[checkboxName].Count > 0)
                    {
                        // The checkbox was checked, and the value is "true" meaning thet have selected and incorrect answer                                               
                        // decrement the correctAnswers count,
                        correctAnswers--;
                    }
                }
                //If the number of correct answers is equal to the number of correct answers for the question then the question is correct
                if (correctAnswerIndices.Count == correctAnswers)
                {
                    correctQuestions++;
                }
            }

            // Calculate the score as a percentage
            double scorePercentage = (double)correctQuestions / Questions.Count * 100;

            // Create a new result and save it
            var result = new Result
            {
                TestId = _resultService.GetNextTestId(),
                Date = DateTime.Now,
                ScorePercentage = scorePercentage
            };
            _resultService.SaveResult(result);

            // Redirect the user to a results page
            return RedirectToPage("./Results");
        }




    }
}
