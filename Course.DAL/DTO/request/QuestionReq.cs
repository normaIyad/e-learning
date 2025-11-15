using Course.DAL.Models;
using System.Text.Json.Serialization;

namespace Course.DAL.DTO.request
{
    public class QuestionReq
    {
        public string QustionText { get; set; }
        public decimal Points { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public QustionType QustionType { get; set; }
        public List<QuestionOptionReq>? QustionOptions { get; set; }
    }
}
