namespace Revise.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public List<StudyTopic> Topics { get; set; }
    }
}
