using System.ComponentModel.DataAnnotations;

namespace Course.DAL.DTO.request
{
    public class ExamReq
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(300)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
        [Required]
        [Range(1, 120)]
        public int DurationInMinutes { get; set; }
    }
}
