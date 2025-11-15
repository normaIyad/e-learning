namespace Course.DAL.DTO.request
{
    public class ExamResultReq
    {
        public int ExamId { get; set; }
        public string UserId { get; set; }
        public decimal Score { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime DateTaken { get; set; }
    }
}
