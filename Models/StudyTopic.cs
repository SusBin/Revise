namespace Revise.Models
{
    public class StudyTopic
    {
        public string Topic { get; set; }
        public List<RevisionQuestion> Questions { get; set; }
    }
}
