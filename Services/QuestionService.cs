using Revise.Models;
using System.Text.Json;

namespace Revise.Services
{
    public class QuestionService
    {
        private List<Course> _courses;

        public QuestionService()
        {
            var json = File.ReadAllText("questions.json");
            _courses = JsonSerializer.Deserialize<List<Course>>(json);
        }

        public List<RevisionQuestion> GetQuestionsByTopic(string topicName)
        {
            return _courses.SelectMany(c => c.Topics)
                           .Where(t => t.Topic == topicName)
                           .SelectMany(t => t.Questions)
                           .ToList();
        }

        public List<string> GetUniqueTopics()
        {
            return _courses.SelectMany(c => c.Topics)
                           .Select(t => t.Topic)
                           .Distinct()
                           .ToList();
        }

        public RevisionQuestion GetQuestionById(int courseId, string topicName, int id)
        {
            return _courses.FirstOrDefault(c => c.Id == courseId)
                           ?.Topics.FirstOrDefault(t => t.Topic == topicName)
                           ?.Questions.FirstOrDefault(q => q.Id == id);
        }
        public Course GetCourseById(int courseId)
        {
            return _courses.FirstOrDefault(c => c.Id == courseId);
        }
        public Dictionary<int, string> GetCourses()
        {
            return _courses.ToDictionary(c => c.Id, c => c.CourseName);
        }

        public List<string> GetTopicsByCourseId(int id)
        {
            var course = _courses.FirstOrDefault(c => c.Id == id);
            return course?.Topics.Select(t => t.Topic).ToList();
        }

        public void UpdateQuestion(int courseId, string topicName, RevisionQuestion updatedQuestion)
        {
            var course = _courses.FirstOrDefault(c => c.Id == courseId);
            if (course != null)
            {
                var topic = course.Topics.FirstOrDefault(t => t.Topic == topicName);
                if (topic != null)
                {
                    var index = topic.Questions.FindIndex(q => q.Id == updatedQuestion.Id);
                    if (index != -1)
                    {
                        topic.Questions[index] = updatedQuestion;
                        var newJson = JsonSerializer.Serialize(_courses);
                        System.IO.File.WriteAllText("questions.json", newJson);
                    }
                }
            }
        }
    }
}
