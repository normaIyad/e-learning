namespace Course.DAL.DTO.Responce
{
    public class ExamResultWithDetails
    {
        public int ExamId { get; set; }
        public string ExamTitle { get; set; }
        public decimal Score { get; set; }
        public DateTime DateTaken { get; set; }
        public string userId { get; set; }
        public string UserName { get; set; }
        public List<ExsamQustionWithAnsRes> ExamQuestion { get; set; }

    }
}
