using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Course.Pl.Areas.User.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("User")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService service;
        private readonly IStudentAnswersService answersService;
        private readonly IQuestionService questionService;

        public ExamController (IExamService service, IStudentAnswersService answersService, IQuestionService questionService)
        {
            this.service=service;
            this.answersService=answersService;
            this.questionService=questionService;
        }
        [HttpGet("CourseExams/{courseId}")]
        public async Task<IActionResult> GetCourseExams (int courseId)
        {
            var exams = await service.GetExams(courseId);
            return Ok(exams);
        }
        [HttpGet("ExamDetails/{examId}")]
        public async Task<IActionResult> GetExamDetails (int examId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type=="Id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in claims.");
            }
            var exam = await service.GetByIdAsync(examId);
            return exam==null ? NotFound() : Ok(exam);
        }
        [HttpPost("SubmitExam/{examId}")]
        public async Task<IActionResult> SubmitExam (int examId, [FromBody] ExamSubmissionReq answers)
        {
            if (answers==null||answers.Answers.Count==0)
            {
                return BadRequest("Answers are required.");
            }
            var userId = User.Claims.FirstOrDefault(c => c.Type=="Id")?.Value;
            var result = await answersService.SubmitAnswersAsync(answers, userId, examId);
            return result ? Ok("Exam submitted successfully") : (IActionResult)StatusCode(StatusCodes.Status500InternalServerError, "Error submitting exam");
        }
        [HttpGet("ExamQuestions/{examId}")]
        public async Task<IActionResult> GetExamQuestions (int examId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type=="Id")?.Value;
            var questions = await questionService.ExamQuestions(examId, userId);
            return questions is not null ? Ok(questions) : BadRequest("not found or qustions dont add yet ");
        }
        [HttpGet("ExamResults/{examId}")]
        public async Task<IActionResult> GetExamResults (int examId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type=="Id")?.Value;
            var results = await answersService.GetResultAsync(examId, userId);
            return results==null ? BadRequest("Results not found.") : Ok(results);
        }
        [HttpGet("ExsamWithReslutsDetals/{examId}")]
        public async Task<IActionResult> GetExsamWithReslutsDetals (int examId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type=="Id")?.Value;
            var results = await answersService.GetResultWithDetailsAsync(examId, userId);
            return results==null ? BadRequest("Results not found.") : Ok(results);
        }
    }
}
