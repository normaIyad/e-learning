using System.ComponentModel.DataAnnotations;

namespace Course.DAL.DTO.request
{
    public class LogInReq
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [MaxLength(16, ErrorMessage = "Password cannot exceed 16 characters")]
        public string Password { get; set; }
    }
}
