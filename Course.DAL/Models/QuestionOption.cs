namespace Course.DAL.Models
{
    public class QuestionOption : BaseModel
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string OptionText { get; set; }
        public bool IsCorrect { get; set; }
        public ICollection<StudentAnswers> StudentAnswerses { get; set; }
    }
}
