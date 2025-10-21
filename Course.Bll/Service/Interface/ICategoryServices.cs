using Course.Bll.Service.GenralIService;
using Course.DAL.DTO.Request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;

namespace Course.Bll.Service.Interface
{
    public interface ICategoryServices : IGeneralService<CategoryReq, CategoryRes, Category>
    {
        //Task GetByIdWithCoursesAsync (int id);

        //Task<IEnumerable<CategoryRes>> GetAllWithCoursesAsync ();
        //Task<CategoryRes> GetByIdWithCoursesAsync (int id);
        //Task<int> AddCategoryAsync (CategoryReq categoryReq);
        // Task UpdateAsync (Category category);
    }
}
