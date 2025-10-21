using Microsoft.EntityFrameworkCore;

namespace Course.DAL.Models
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }
    public enum PaymentMethod
    {
        Visa,
        Cash,
        free
    }
    public class Payment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public int CourseId { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public PaymentMethod PaymentMethod { get; set; }
        public string? PaymentId { get; set; }
        public PaymentStatus Status { get; set; }

    }
}
