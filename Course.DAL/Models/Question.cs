namespace Course.DAL.Models
{
    public enum QustionType
    {
        MultipleChoice,
        TrueFalse,
        ShortAnswer
    }
    public class Question : BaseModel
    {
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public string QustionText { get; set; }
        public decimal Points { get; set; }
        public QustionType QustionType { get; set; }
        public ICollection<QuestionOption> QustionOptions { get; set; }
        public ICollection<StudentAnswers> StudentAnswers { get; set; }
    }
}
