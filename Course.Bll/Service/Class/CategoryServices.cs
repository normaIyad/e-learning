using Course.Bll.Service.GenralIService;
using Course.Bll.Service.Interface;
using Course.DAL.DTO.Request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;
using Course.DAL.Repositry;
using Mapster;

namespace Course.Bll.Service.Class
{
    public class CategoryServices : GeneralService<CategoryReq, CategoryRes, Category>, ICategoryServices
    {
        private readonly ICategoryRepo categoryRepo;
        private readonly IFileService fileService;

        public CategoryServices (ICategoryRepo categoryRepo, IFileService fileService) : base(categoryRepo)
        {
            this.categoryRepo=categoryRepo;
            this.fileService=fileService;
        }

        public async Task<int> AddCategoryAsync (CategoryReq categoryReq)
        {
            var category = categoryReq.Adapt<Category>();
            if (categoryReq.ImgeUrl!=null)
            {
                var imgUrl = await fileService.UploadFileAsync(categoryReq.ImgeUrl, "Categorylings");
                category.ImgeUrl=imgUrl;
            }
            await categoryRepo.AddAsync(category);
            return 1;
        }

        public async Task<IEnumerable<CategoryRes>> GetAllWithCoursesAsync (string url)
        {
            var categories = await categoryRepo.GetAllAsync();
            var categoriesRes = categories.Adapt<List<CategoryRes>>();

            foreach (var category in categoriesRes)
            {
                if (!string.IsNullOrEmpty(category.ImgeUrl))
                {
                    // Note: Based on your wwwroot structure, images are directly in Categorylings
                    category.ImgeUrl=url+"CategoryImgs/"+category.ImgeUrl;
                }
            }

            return categoriesRes;
        }

        public async Task<CategoryRes> GetByIdWithCatigoryAsync (int id, string url)
        {
            var category = await categoryRepo.GetByIdAsync(id);
            var categoryRes = category.Adapt<CategoryRes>();
            if (!string.IsNullOrEmpty(categoryRes.ImgeUrl))
            {
                categoryRes.ImgeUrl=url+"CategoryImgs/"+categoryRes.ImgeUrl;
            }
            return categoryRes;
        }

        public new async Task UpdateAsync (int id, CategoryReq category)
        {
            if (category==null)
            {
                throw new ArgumentNullException(nameof(category));
            }
            var existingCategory = await categoryRepo.GetByIdAsync(id);
            if (existingCategory==null)
            {
                throw new Exception("Category not found.");
            }
            var categoryToUpdate = category.Adapt<Category>();
            if (category.ImgeUrl!=null)
            {
                // Delete old image
                await fileService.DeleteFileAsync(existingCategory.ImgeUrl, "CategoryImgs");
                // Upload new image
                var imgUrl = await fileService.UploadFileAsync(category.ImgeUrl, "CategoryImgs");
                categoryToUpdate.ImgeUrl=imgUrl;
            }
            await categoryRepo.UpdateAsync(categoryToUpdate);
            return;
        }

        public new async Task<bool> DeleteAsync (int id)
        {
            var category = await categoryRepo.GetByIdAsync(id);
            if (category==null)
                return false;
            if (category.ImgeUrl is not null)
            {
                await fileService.DeleteFileAsync(category.ImgeUrl, "CategoryImgs");
            }
            var deleted = await categoryRepo.DeleteAsync(category);
            return deleted>0;
        }
    }
}