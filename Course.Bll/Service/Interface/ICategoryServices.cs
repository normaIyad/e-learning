using Course.Bll.Service.GenralIService;
using Course.DAL.DTO.Request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;

namespace Course.Bll.Service.Interface
{
    public interface ICategoryServices : IGeneralService<CategoryReq, CategoryRes, Category>
    {
        Task<IEnumerable<CategoryRes>> GetAllWithCoursesAsync (string url);

        //Task<CategoryRes> GetByIdWithCoursesAsync (int id);
        Task<CategoryRes> GetByIdWithCatigoryAsync (int id, string url);

        Task<int> AddCategoryAsync (CategoryReq categoryReq);
        Task UpdateAsync (int id, CategoryReq category);
    }
}
