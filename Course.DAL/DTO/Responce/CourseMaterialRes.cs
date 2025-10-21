namespace Course.DAL.DTO.Responce
{
    public class CourseMaterialRes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public string? MaterialUrl { get; set; }
        public string MaterialType { get; set; } // e.g., "Video", "Document", etc.
        public int CourseId { get; set; }
    }
}
