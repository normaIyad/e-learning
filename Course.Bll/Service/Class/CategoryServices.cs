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
        //private readonly ICategoryRepo _categoryRepo;

        public CategoryServices (ICategoryRepo categoryRepo) : base(categoryRepo)
        {

        }

        //public async Task<int> AddCategoryAsync (CategoryReq categoryReq)
        //{
        //    var category = categoryReq.Adapt<Category>();
        //    await AddAsync(category);
        //    return category.Id;
        //}

        //public async Task<IEnumerable<CategoryRes>> GetAllWithCoursesAsync ()
        //{
        //    var categories = await GetAllAsync();
        //    return categories.Adapt<IEnumerable<CategoryRes>>();
        //}

        //public async Task<CategoryRes?> GetByIdWithCoursesAsync (int id)
        //{
        //    var category = await GetByIdAsync(id);
        //    return category?.Adapt<CategoryRes>();
        //}

    }
}
