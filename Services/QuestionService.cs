using Revise.Models;
using System.Text.Json;

namespace Revise.Services
{
    public class QuestionService
    {
        private List<RevisionQuestion> _questions;

        public QuestionService()
        {
            var json = File.ReadAllText("questions.json");
            _questions = JsonSerializer.Deserialize<List<RevisionQuestion>>(json);
        }

        public List<RevisionQuestion> GetQuestionsByTopic(string topic)
        {
            return _questions.Where(q => q.Topic == topic).ToList();
        }

        public List<string> GetUniqueTopics()
        {
            return _questions.Select(q => q.Topic).Distinct().ToList();
        }
        public RevisionQuestion GetQuestionById(int id)
        {
            return _questions.FirstOrDefault(q => q.Id == id);
        }

        public void UpdateQuestion(RevisionQuestion updatedQuestion)
        {
            var index = _questions.FindIndex(q => q.Id == updatedQuestion.Id);
            if (index != -1)
            {
                _questions[index] = updatedQuestion;
                var newJson = JsonSerializer.Serialize(_questions);
                System.IO.File.WriteAllText("questions.json", newJson);
            }
        }

    }
}
