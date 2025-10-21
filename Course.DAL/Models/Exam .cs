namespace Course.DAL.Models
{
    public class Exam : BaseModel
    {
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public int DurationInMinutes { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();

    }
}
