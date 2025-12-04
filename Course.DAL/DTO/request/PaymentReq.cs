using Course.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Course.DAL.DTO.request
{
    public class PaymentReq
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public int CourseId { get; set; }

    }
}
