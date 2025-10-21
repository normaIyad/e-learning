using Course.DAL.DataBase;

namespace Course.DAL.Repositry.Class
{
    public class CourseRepositry : GenralRepositry<Models.Course>, ICourseRepositry
    {
        public CourseRepositry (ApplicationDbContext context) : base(context)
        {
        }
    }
}
