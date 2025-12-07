using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Course.DAL.DTO.request
{
    public class CourseReq
    {
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Title { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        [Required]
        [Range(1, 100)]
        public int DurationInHours { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.00, double.MaxValue)]
        public decimal Price { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive integer.")]
        public int CategoryId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        [MaxLength(50)]
        public string InstructorId { get; set; }
        [Required]
        public IFormFile ImgeUrl { get; set; }
    }
}
