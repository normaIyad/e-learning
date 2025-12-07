using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization.Formatters;

namespace Course.DAL.DTO.Request
{
    public class CategoryReq
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }

        [Required]
        public IFormFile ImgeUrl { get; set; }

    }
}
