using Course.DAL.DataBase;
using Course.DAL.Models;

namespace Course.DAL.Repositry.Class
{
    public class EnrollmentsRepo : GenralRepositry<Enrollment>, IEnrollmentsRepo
    {
        private readonly ApplicationDbContext context;

        public EnrollmentsRepo (ApplicationDbContext context) : base(context)
        {
            this.context=context;
        }


        public Task<bool> IsUserEnrollToCource (int courseId, string UserId)
        {
            var isEnrolled = context.Enrollments.Any(e => e.CourseId==courseId&&e.UserId==UserId);
            return Task.FromResult(isEnrolled);
        }
    }
}
