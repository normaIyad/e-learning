namespace Course.DAL.DTO.request
{
    public class StudentAnswersReq
    {
        public int QuestionId { get; set; }
        public int? QuestionOptionId { get; set; }
        public string? AnswerText { get; set; }
    }
}
