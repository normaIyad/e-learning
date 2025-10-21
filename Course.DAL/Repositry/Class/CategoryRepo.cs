using Course.DAL.DataBase;
using Course.DAL.Models;

namespace Course.DAL.Repositry.Class
{
    public class CategoryRepo : GenralRepositry<Category>, ICategoryRepo
    {
        public CategoryRepo (ApplicationDbContext context) : base(context)
        {
        }

    }
}
