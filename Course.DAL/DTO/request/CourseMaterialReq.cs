using Course.DAL.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Course.DAL.DTO.request
{
    public class CourseMaterialReq
    {
        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Title { get; set; }
        [MaxLength(500)]
        [MinLength(3)]
        public string Description { get; set; }
        public IFormFile? MaterialUrl { get; set; }
        [Required]
        public MaterialType MaterialType { get; set; } // e.g., "Video", "Document", etc.

    }
}
