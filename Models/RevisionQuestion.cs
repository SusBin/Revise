namespace Revise.Models
{
    public class RevisionQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<Answer> Answers { get; set; }
        public string Topic { get; set; }
    }

    public class Answer
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
