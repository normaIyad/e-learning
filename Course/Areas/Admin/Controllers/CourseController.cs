using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Course.Pl.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Admin")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin , Admin , Instructor")]

    public class CourseController : ControllerBase
    {
        private readonly ICourseService service;
        private readonly ICourseMaterialService materialService;

        public CourseController (ICourseService service, ICourseMaterialService materialService)
        {
            this.service=service;
            this.materialService=materialService;
        }
        [HttpGet]
        public async Task<IActionResult> Get ()
        {
            var url = $"{Request.Scheme}://{Request.Host}/";
            var courses = await service.GetAllCourses(url);
            return Ok(courses);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get (int id)
        {
            var url = $"{Request.Scheme}://{Request.Host}/";
            var course = await service.GetById(id, url);
            return course==null ? NotFound() : Ok(course);
        }
        [HttpPost]
        public async Task<IActionResult> Create ([FromForm] CourseReq course)
        {
            if (course==null)
                return BadRequest("Course data is required.");

            await service.addCourse(course);
            return Ok("Course added successfully");
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update (int id, [FromForm] CourseReq course)
        {
            if (course==null) return BadRequest();
            var existingCourse = await service.GetByIdAsync(id);
            if (existingCourse==null) return NotFound();
            await service.updateCourse(id, course);
            return NoContent();
        }
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete (int id)
        {
            var course = await service.removeCourse(id);
            return NoContent();
        }
        [HttpGet("{id}/CourseMaterials")]
        public async Task<IActionResult> GetCourseMaterials (int id)
        {
            var url = $"{Request.Scheme}://{Request.Host}/";
            var materials = await service.GetCourseWithMaterialsAsync(id, url);
            return Ok(materials);
        }
        [HttpPost("{id}/CourseMaterial")]
        public async Task<IActionResult> AddCourseMaterial ([FromForm] CourseMaterialReq req, int id)
        {
            var instructorId = User.FindFirstValue("Id");
            var isInstructor = await materialService.IsInstrctorCanAddMatirial(id, instructorId);
            if (!isInstructor) return Forbid("You are not authorized to add material.");

            if (req==null||req.MaterialUrl==null)
                return BadRequest("Course material file is required.");

            await materialService.AddCourseMaterialAsync(req, id);
            return Ok("Course material added successfully.");
        }
        [HttpDelete("{id}/CourseMaterial")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> DeleteCourseMaterial (int id)
        {
            var userId = User.FindFirstValue("Id");
            var isInstracter = await materialService.IsInstrctorCanAddMatirial(id, userId);
            if (!isInstracter)
            {
                return Forbid("You are not authorized to delete material from this course.");
            }
            var result = await materialService.DeleteAsync(id);
            return !result ? NotFound("Course material not found.") : Ok("Course material deleted successfully.");
        }
        [HttpPut("{id}/CourseMaterial")]
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> UpdateCourseMaterial (int id, [FromQuery] int courseMaterialId, [FromForm] CourseMaterialReq req)
        {
            var userId = User.FindFirstValue("Id");
            var isInstructor = await materialService.IsInstrctorCanAddMatirial(id, userId);
            if (!isInstructor) return Forbid("You are not authorized to update this material.");

            if (req==null) return BadRequest("Course material data is required.");

            var result = await materialService.UpdateMaterialAsync(courseMaterialId, req);
            return !result ? NotFound("Course material not found.") : Ok("Course material updated successfully.");
        }


    }
}
