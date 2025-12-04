namespace Course.DAL.DTO.Responce
{
    public class ExamStatisticsDto
    {
        public string ExamTitle { get; set; }
        public int TotalStudents { get; set; }
        public int MaxPossibleScore { get; set; }

        // Percentage-based (0-100%)
        public double AverageScore { get; set; }
        public int HighestScore { get; set; }
        public int LowestScore { get; set; }
        public double PassRate { get; set; }

        // Raw scores
        public double AverageRawScore { get; set; }
        public int HighestRawScore { get; set; }
        public int LowestRawScore { get; set; }

        public bool IsPercentageBased { get; set; }
        public List<ScoreRangeDto> ScoreDistribution { get; set; }
        public List<MissedQuestionDto> MostMissedQuestions { get; set; }
    }

    public class ScoreRangeDto
    {
        public string Range { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class MissedQuestionDto
    {
        public string QuestionText { get; set; }
        public int QuestionId { get; set; }
        public double IncorrectPercentage { get; set; }
        public int TotalAttempts { get; set; }
        public int IncorrectAttempts { get; set; }
    }

}
