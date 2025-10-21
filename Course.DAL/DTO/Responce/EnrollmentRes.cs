namespace Course.DAL.DTO.Responce
{
    public class EnrollmentRes
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public int CourseDurationInHours { get; set; }
        public decimal CoursePrice { get; set; }
        public string InstructorId { get; set; }
        public string InstructorName { get; set; }

    }
}
