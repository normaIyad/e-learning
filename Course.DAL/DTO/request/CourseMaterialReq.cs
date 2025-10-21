using Course.DAL.Models;
using Microsoft.AspNetCore.Http;

namespace Course.DAL.DTO.request
{
    public class CourseMaterialReq
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? MaterialUrl { get; set; }
        public MaterialType MaterialType { get; set; } // e.g., "Video", "Document", etc.
    }
}
