namespace Course.DAL.Models
{
    public class ExamResult : BaseModel
    {
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public decimal Score { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateTaken { get; set; }
    }
}
