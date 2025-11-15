using Course.DAL.Models;

namespace Course.DAL.DTO.Responce
{
    public class ExsamQustionWithAnsRes
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public QustionType QuestionType { get; set; }
        public string QuestionTyp => QuestionType.ToString();
        public string? AnswerText { get; set; }
        public List<QuestionOptionIsSelectedRes>? Options { get; set; }
    }
}
