using Microsoft.EntityFrameworkCore;

namespace Course.DAL.Models
{
    [PrimaryKey(nameof(UserId), nameof(CourseId))]
    public class Enrollment
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
