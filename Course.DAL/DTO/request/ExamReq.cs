namespace Course.DAL.DTO.request
{
    public class ExamReq
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
