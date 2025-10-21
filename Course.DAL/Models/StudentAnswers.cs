namespace Course.DAL.Models
{
    public class StudentAnswers : BaseModel
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string userId { get; set; }
        public ApplicationUser User { get; set; }
        public int? QuestionOptionId { get; set; }
        public QuestionOption? QuestionOption { get; set; }
        public string? AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public decimal PointsEarned { get; set; }

    }
}
