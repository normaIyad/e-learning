using System.ComponentModel.DataAnnotations;

namespace Course.DAL.DTO.request
{
    public class StudentAnswersReq
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "QuestionId must be a positive integer.")]
        public int QuestionId { get; set; }
        public int? QuestionOptionId { get; set; }
        public string? AnswerText { get; set; }
    }
}
