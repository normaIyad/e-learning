using System.ComponentModel.DataAnnotations;

namespace Course.DAL.DTO.Request
{
    public class CategoryReq
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
    }
}
