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
            var categories = await _categoryServices.GetAllAsync();
            return Ok(categories);
        }
        [HttpGet("debug-claims")]
        public IActionResult DebugClaims ()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
            return Ok(claims);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById (int id)
        {
            var category = await _categoryServices.GetByIdAsync(id);
            return category==null ? NotFound() : Ok(category);
        }
    }
}
