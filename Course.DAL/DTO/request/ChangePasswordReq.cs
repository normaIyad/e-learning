using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Course.DAL.DTO.request
{
    public class ChangePasswordReq
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(16)]
        [PasswordPropertyText]
        public string OldPassword { get; set; }
        [Required]
        [MaxLength(16)]
        [MinLength(6)]
        [PasswordPropertyText]
        public string NewPassword { get; set; }
    }
}
