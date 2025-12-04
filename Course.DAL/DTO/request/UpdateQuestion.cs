using Course.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Course.DAL.DTO.request
{
    public class UpdateQuestion
    {
        [Required]
        [MaxLength(500)]
        [MinLength(3)]
        public string QustionText { get; set; }
        [Required]
        [Range(0, 100)]
        public decimal Points { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required]
        public QustionType QustionType { get; set; }

    }
}
