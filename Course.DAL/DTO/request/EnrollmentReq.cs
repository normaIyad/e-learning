using System.ComponentModel.DataAnnotations;

namespace Course.DAL.DTO.request
{
    public class EnrollmentReq
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "CourseId must be a positive integer.")]
        public int CourseId { get; set; }
    }
}
