using Course.Bll.Service.Interface;
using Course.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Course.Pl.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Admin")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin , Admin ")]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById (int id)
        {
            var category = await _categoryServices.GetByIdAsync(id);
            return category==null ? NotFound() : Ok(category);
        }

        [HttpPost("AddCatigory")]
        public async Task<IActionResult> Create ([FromBody] CategoryReq categoryReq)
        {
            if (categoryReq==null)
                return BadRequest("Category data is required.");
            await _categoryServices.AddAsync(categoryReq);
            return Ok("Category add ");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update (int id, [FromBody] CategoryReq categoryReq)
        {
            if (categoryReq==null) return BadRequest();

            var existingCategory = await _categoryServices.GetByIdAsync(id);
            if (existingCategory==null) return NotFound();
            await _categoryServices.UpdateAsync(id, categoryReq);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            var category = await _categoryServices.DeleteAsync(id);
            return NoContent();
        }

    }
}
