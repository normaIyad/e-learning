using Course.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Course.DAL.DataBase
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Course.DAL.Models.Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<CourseMaterial> CourseMaterials { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<StudentAnswers> StudentAnswers { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Identity table names

            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");


            // Decimal precision

            modelBuilder.Entity<Course.DAL.Models.Course>()
                .Property(c => c.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ExamResult>()
                .Property(er => er.Score)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Question>()
                .Property(q => q.Points)
                .HasPrecision(18, 2);

            modelBuilder.Entity<StudentAnswers>()
                .Property(sa => sa.PointsEarned)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);


            // Enrollment relationships

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade from user

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Safe to cascade (no cycle)

            modelBuilder.Entity<Enrollment>()
                .HasIndex(e => new { e.UserId, e.CourseId })
                .IsUnique();


            // Payment relationships

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany() // optional: add ICollection<Payment> to ApplicationUser if needed
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade loop

            //modelBuilder.Entity<Payment>()
            //    .HasOne(p => p.Enrollment)
            //    .WithMany(e => e.Payments)
            //    .HasForeignKey(p => p.EnrollmentId)
            //    .OnDelete(DeleteBehavior.Cascade); // cascade when enrollment deleted


            // ExamResult and StudentAnswers

            modelBuilder.Entity<ExamResult>()
                .HasOne(er => er.User)
                .WithMany()
                .HasForeignKey(er => er.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentAnswers>()
                .HasOne(sa => sa.User)
                .WithMany()
                .HasForeignKey(sa => sa.userId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
