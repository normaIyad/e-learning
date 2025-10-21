using Microsoft.AspNetCore.Identity;

namespace Course.DAL.Models
{
    public enum Roles
    {
        SuperAdmin,
        Admin,
        User,
        Instructor
    }
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
