namespace Course.DAL.DTO.Responce
{
    public class CourseRes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationInHours { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public string InstructorId { get; set; }
        public string InstructorName { get; set; }
        public string ImgeUrl { get; set; }
        }
}
