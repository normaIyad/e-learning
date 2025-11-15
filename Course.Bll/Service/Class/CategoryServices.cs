using Course.Bll.Service.GenralIService;
using Course.Bll.Service.Interface;
using Course.DAL.DTO.Request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;
using Course.DAL.Repositry;

namespace Course.Bll.Service.Class
{
    public class CategoryServices : GeneralService<CategoryReq, CategoryRes, Category>, ICategoryServices
    {
        public CategoryServices (ICategoryRepo categoryRepo) : base(categoryRepo)
        {

        }
    }
}
