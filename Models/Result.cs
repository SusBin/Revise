namespace Revise.Models
{
    public class Result
    {
        public int TestId { get; set; }
        public DateTime Date { get; set; }
        public double ScorePercentage { get; set; }
        public Dictionary<int, List<int>> IncorrectQuestions { get; set; }
    }

}
