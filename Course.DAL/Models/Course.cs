namespace Course.DAL.Models
{
    public class Course : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationInHours { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; } = true;
        public string InstructorId { get; set; }
        public string ImgeUrl { get; set; }
        public ApplicationUser Instructor { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<CourseMaterial> CourseMaterials { get; set; }
        public ICollection<Exam> Exams { get; set; }
        public Category Category { get; set; }
        public bool IsFree => Price==0;
    }
}
