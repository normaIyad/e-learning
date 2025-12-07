using Course.Bll.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Course.Pl.Areas.User.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("User")]
    //[Authorize(Policy = "UserRole")]
    [ApiController]
    // [Authorize(Roles = "User , Instructor")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController (ICategoryServices categoryServices)
        {
            _categoryServices=categoryServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll ()
        {
            var url = $"{Request.Scheme}://{Request.Host}/";
            var categories = await _categoryServices.GetAllWithCoursesAsync(url);
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById (int id)
        {
            var url = $"{Request.Scheme}://{Request.Host}/";
            var category = await _categoryServices.GetByIdWithCatigoryAsync(id, url);
            return category==null ? NotFound() : Ok(category);
        }
    }
}
