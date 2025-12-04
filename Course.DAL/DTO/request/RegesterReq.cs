using System.ComponentModel.DataAnnotations;

namespace Course.DAL.DTO.request
{
    public class RegesterReq
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [MaxLength(16, ErrorMessage = "Password cannot exceed 16 characters")]
        public string Password { get; set; }
    }
}
