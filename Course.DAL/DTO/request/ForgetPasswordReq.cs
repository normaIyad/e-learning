using System.ComponentModel.DataAnnotations;

namespace Course.DAL.DTO.request
{
    public class ForgetPasswordReq
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
    }
}
