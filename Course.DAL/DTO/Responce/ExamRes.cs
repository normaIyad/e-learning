namespace Course.DAL.DTO.Responce
{
    public class ExamRes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime Date { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
