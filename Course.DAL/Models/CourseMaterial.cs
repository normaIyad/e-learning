namespace Course.DAL.Models
{
    public enum MaterialType
    {
        Video,
        Document,
        Audio,
        Image,
        Other
    }
    public class CourseMaterial : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public string? MaterialUrl { get; set; }
        public MaterialType MaterialType { get; set; } // e.g., "Video", "Document", etc.
        public int CourseId { get; set; }
        public Course Course { get; set; }

    }
}
