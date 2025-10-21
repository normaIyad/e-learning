using Course.Bll.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Course.Pl.Areas.User.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("User")]
    [ApiController]

    public class CourseController : ControllerBase
    {
        private readonly ICourseService service;

        public CourseController (ICourseService service)
        {
            this.service=service;
        }
        [HttpGet("Courses")]
        public async Task<IActionResult> Get ()
        {
            var courses = await service.GetAllAsync();
            return Ok(courses);
        }
        [HttpGet("Course/{id}")]
        public async Task<IActionResult> Get (int id)
        {
            var course = await service.GetByIdAsync(id);
            return course==null ? NotFound() : Ok(course);
        }
        [Authorize(Roles = "User")]
        [HttpGet("WithMaterials/{id}")]
        public async Task<IActionResult> GetCourseWithMaterials (int id)
        {
            var userId = User.FindFirstValue("Id");
            var isEnrolled = await service.IsUserEnrollToCource(id, userId);
            if (!isEnrolled)
                return RedirectToAction(nameof(Get), new { id });

            var courseWithMaterials = await service.GetCourseWithMaterialsAsync(id);
            return courseWithMaterials==null ? NotFound() : Ok(courseWithMaterials);
        }
    }
}
