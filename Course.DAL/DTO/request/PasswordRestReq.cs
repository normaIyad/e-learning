using System.ComponentModel.DataAnnotations;
namespace Course.DAL.DTO.request
{
    public class PasswordRestReq
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "Password cannot exceed 16 characters")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string PasswordRestCode { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(16, ErrorMessage = "Password cannot exceed 16 characters")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string NewPassword { get; set; }
    }
}
