namespace Course.DAL.DTO.Responce
{
    public class CourseWithMatirialResponce
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int DurationInHours { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; } = true;
        public string InstructorId { get; set; }
        public string ImgeUrl { get; set; }
        public ICollection<CourseMaterialRes> CourseMaterials { get; set; }
        //  public ICollection<Exam> Exams { get; set; }

    }
}
