using Revise.Models;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

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

        public RevisionQuestion GetQuestionById(int id)
        {
            return _courses.SelectMany(c => c.Topics.SelectMany(t => t.Questions))
                           .FirstOrDefault(q => q.Id == id);
        }

        public RevisionQuestion GetQuestionById(int courseId, int id)
        {
            return _courses.FirstOrDefault(c => c.Id == courseId)
                           ?.Topics.SelectMany(t => t.Questions)
                           .FirstOrDefault(q => q.Id == id);
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
                    var question = topic.Questions.FirstOrDefault(q => q.Id == updatedQuestion.Id);
                    if (question != null)
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
        public int GetNextQuestionId(int courseId)
        {
            // Get the course by Id
            var course = _courses.FirstOrDefault(c => c.Id == courseId);

            // Check if the course exists
            if (course == null)
            {
                throw new Exception("Course not found.");
            }

            // Get all questions for the course
            var questions = course.Topics.SelectMany(t => t.Questions).ToList();

            // Check if there are any questions
            if (!questions.Any())
            {
                throw new Exception("No questions available for this course.");
            }

            // Get the last question
            var lastQuestion = questions.Last();

            // Assuming the questions have an Id property
            var lastQuestionId = lastQuestion.Id;

            // Get the next question Id
            var nextQuestionId = lastQuestionId + 1;

            return nextQuestionId;
        }

        public void AddNewQuestion(int courseId, string topicName, RevisionQuestion newQuestion)
        {
            var course = GetCourseById(courseId);
            if (course != null)
            {
                // Set the Id of the new question
                newQuestion.Id = GetNextQuestionId(courseId);
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

        public RevisionQuestion GetRandomQuestion(int courseId)
        {
            // Get the course by Id
            var course = _courses.FirstOrDefault(c => c.Id == courseId);

            // Check if the course exists
            if (course == null)
            {
                throw new Exception("Course not found.");
            }

            // Get all questions for the course
            var questions = course.Topics.SelectMany(t => t.Questions).ToList();

            // Check if there are any questions
            if (!questions.Any())
            {
                throw new Exception("No questions available for this course.");
            }

            // Use System.Random to generate a random index
            var random = new System.Random();
            var randomIndex = random.Next(questions.Count);

            // Return the randomly selected question
            return questions[randomIndex];
        }

        public Test GenerateTest(int courseId, int numQuestions)
        {
            var test = new Test { Questions = new List<RevisionQuestion>() };
            for (int i = 0; i < numQuestions; i++)
            {
                var question = GetRandomQuestion(courseId);
                test.Questions.Add(question);
            }
            return test;
        }

        public void DeleteQuestion(int courseId, string topicName, int id)
        {
            var course = _courses.FirstOrDefault(c => c.Id == courseId);
            if (course != null)
            {
                var topic = course.Topics.FirstOrDefault(t => t.Topic == topicName);
                if (topic != null)
                {
                    var question = topic.Questions.FirstOrDefault(q => q.Id == id);
                    if (question != null)
                    {
                        topic.Questions.Remove(question);
                        var newJson = JsonSerializer.Serialize(_courses);
                        System.IO.File.WriteAllText("questions.json", newJson);
                    }
                }
            }
        }

        public void DeleteTopic(int courseId, string topicName)
        {
            var course = _courses.FirstOrDefault(c => c.Id == courseId);
            if (course != null)
            {
                var topic = course.Topics.FirstOrDefault(t => t.Topic == topicName);
                if (topic != null)
                {
                    course.Topics.Remove(topic);
                    var newJson = JsonSerializer.Serialize(_courses);
                    System.IO.File.WriteAllText("questions.json", newJson);
                }
            }
        }

        public void DeleteCourse(int courseId)
        {
            var course = _courses.FirstOrDefault(c => c.Id == courseId);
            if (course != null)
            {
                _courses.Remove(course);
                var newJson = JsonSerializer.Serialize(_courses);
                System.IO.File.WriteAllText("questions.json", newJson);
            }
        }


        public void AddNewCourse(Course newCourse)
        {
            var course = _courses.FirstOrDefault(c => c.Id == newCourse.Id);
            if (course == null)
            {
                _courses.Add(newCourse);
                var newJson = JsonSerializer.Serialize(_courses);
                System.IO.File.WriteAllText("questions.json", newJson);
            }
        }

        public void UpdateCourse(Course updatedCourse)
        {
            var course = _courses.FirstOrDefault(c => c.Id == updatedCourse.Id);
            if (course != null)
            {
                var index = _courses.FindIndex(c => c.Id == updatedCourse.Id);
                if (index != -1)
                {
                    _courses[index] = updatedCourse;
                    var newJson = JsonSerializer.Serialize(_courses);
                    System.IO.File.WriteAllText("questions.json", newJson);
                }
            }
        }

        public void UpdateTopic(int courseId, string topicName, StudyTopic updatedTopic)
        {
            var course = _courses.FirstOrDefault(c => c.Id == courseId);
            if (course != null)
            {
                var topic = course.Topics.FirstOrDefault(t => t.Topic == topicName);
                if (topic != null)
                {
                    var index = course.Topics.FindIndex(t => t.Topic == topicName);
                    if (index != -1)
                    {
                        course.Topics[index] = updatedTopic;
                        var newJson = JsonSerializer.Serialize(_courses);
                        System.IO.File.WriteAllText("questions.json", newJson);
                    }
                }
            }
        }

    }
}
