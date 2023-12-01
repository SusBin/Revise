namespace Revise.Models
{
    public class RevisionQuestion
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public List<Answer> Answers { get; set; }
    }

   
}
