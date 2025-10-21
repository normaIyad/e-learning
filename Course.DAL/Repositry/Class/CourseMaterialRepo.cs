using Course.DAL.DataBase;
using Course.DAL.Models;

namespace Course.DAL.Repositry.Class
{
    public class CourseMaterialRepo : GenralRepositry<CourseMaterial>, ICourseMaterialRepo
    {
        public CourseMaterialRepo (ApplicationDbContext context) : base(context)
        {
        }
    }
}
