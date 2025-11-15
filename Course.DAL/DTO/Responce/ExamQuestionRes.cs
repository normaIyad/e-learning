using Course.DAL.Models;
using System.Text.Json.Serialization;

namespace Course.DAL.DTO.Responce
{
    public class ExamQuestionRes
    {
        public int Id { get; set; }
        public string QustionText { get; set; }
        public decimal Points { get; set; }
        [JsonIgnore]
        public QustionType QustionTyp { get; set; }
        public string QustionType => QustionTyp.ToString();
        public ICollection<QuestionOptionExamRes> QustionOptions { get; set; }

    }
}
