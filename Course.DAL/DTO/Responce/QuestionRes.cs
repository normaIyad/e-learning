using Course.DAL.Models;
using System.Text.Json.Serialization;

namespace Course.DAL.DTO.Responce
{
    public class QuestionRes
    {
        public int Id { get; set; }
        public string QustionText { get; set; }
        public decimal Points { get; set; }

        [JsonIgnore]
        public QustionType QustionType { get; set; }

        public string QustionTyp => QustionType.ToString();

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ICollection<QuestionOptionRes>? QustionOptions { get; set; }
    }
}
