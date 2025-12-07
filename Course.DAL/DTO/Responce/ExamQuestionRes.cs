using Course.DAL.Models;
using System.Text.Json.Serialization;

namespace Course.DAL.DTO.Responce
{
    public class ExamQuestionRes
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public decimal Points { get; set; }
        [JsonIgnore]
        public QustionType QuestionType { get; set; }
        public string QuestionTypeString => QuestionType.ToString();
        public ICollection<QuestionOptionExamRes>? QuestionOptions { get; set; }
    }
}