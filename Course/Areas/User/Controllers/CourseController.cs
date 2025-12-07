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
            var url = $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
            var courses = await service.GetAllCourses(url);
            return Ok(courses);
        }
        [HttpGet("Course/{id}")]
        public async Task<IActionResult> Get (int id)
        {
            var url = $"{Request.Scheme}://{Request.Host}/";
            var course = await service.GetById(id, url);
            return course==null ? NotFound() : Ok(course);
        }
        [Authorize(Roles = "User")]
        [HttpGet("WithMaterials/{id}")]
        public async Task<IActionResult> GetCourseWithMaterials (int id)
        {
            var userId = User.FindFirstValue("Id");
            var isEnrolled = await service.IsUserEnrollToCource(id, userId);
            if (!isEnrolled)
                return BadRequest("You are not enrolled in this course.");

            var url = $"{Request.Scheme}://{Request.Host}/";
            var courseWithMaterials = await service.GetCourseWithMaterialsAsync(id, url);
            return courseWithMaterials==null ? NotFound() : Ok(courseWithMaterials);
        }
    }
}
