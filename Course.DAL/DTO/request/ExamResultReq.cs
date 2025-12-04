using System.ComponentModel.DataAnnotations;

namespace Course.DAL.DTO.request
{
    public class ExamResultReq
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "ExamId must be a positive integer.")]
        public int ExamId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        [Range(0.0, 100.0, ErrorMessage = "Score must be between 0 and 100.")]
        public decimal Score { get; set; }
        [Required]
        public bool IsActive { get; set; } = true;
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateTaken { get; set; }
    }
}
