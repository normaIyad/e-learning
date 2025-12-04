using System.ComponentModel.DataAnnotations;

namespace Course.DAL.DTO.request
{
    public class ChangeRoleReq
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
