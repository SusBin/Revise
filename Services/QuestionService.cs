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
        public int GetCourseIdbyTopic(string topicName)
        {
            return (int)(_courses.FirstOrDefault(c => c.Topics.Any(t => t.Topic == topicName))?.Id);
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

        public StudyTopic GetStudyTopic(int courseId, string topicName)
        {
            var course = GetCourseById(courseId);
            return course?.Topics.FirstOrDefault(t => t.Topic == topicName);
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

        public void AddNewTopic(int courseId, string topicName)
        {
            var course = GetCourseById(courseId);
            if (course != null)
            {
                var topic = course.Topics.FirstOrDefault(t => t.Topic == topicName);
                if (topic == null)
                {
                    // If the topic is not found, create a new one
                    topic = new StudyTopic { Topic = topicName, Questions = new List<RevisionQuestion>() };
                    course.Topics.Add(topic);
                    var newJson = JsonSerializer.Serialize(_courses);
                    System.IO.File.WriteAllText("questions.json", newJson);
                }
            }
        }

        public void AddNewQuestion(int courseId, string topicName, RevisionQuestion newQuestion)
        {
            var course = GetCourseById(courseId);
            if (course != null)
            {
                var topic = course.Topics.FirstOrDefault(t => t.Topic == topicName);
                if (topic != null)
                {
                    // If the topic is found, add the new question
                    topic.Questions.Add(newQuestion);
                    var newJson = JsonSerializer.Serialize(_courses);
                    System.IO.File.WriteAllText("questions.json", newJson);
                }
            }
        }

    }
}
