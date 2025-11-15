using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Course.DAL.Repositry.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Course.Pl.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Admin")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin , Admin , Instructor")]
    public class ExamController : ControllerBase
    {
        private readonly IExamService examService;
        private readonly IQuestionService questionService;

        public ExamController (IExamService examService, IQuestionService questionService)
        {
            this.examService=examService;
            this.questionService=questionService;
        }

        // ================================
        // 📘 EXAM MANAGEMENT
        // ================================

        [HttpPost("{courseId}/AddExam")]
        public async Task<IActionResult> AddExam (int courseId, [FromBody] ExamReq examReq)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type=="Id")?.Value;
            if (examReq==null)
                return BadRequest("Exam data is required.");

            var result = await examService.AddExam(examReq, courseId, userId);
            return result
                ? Ok("Exam added successfully")
                : StatusCode(StatusCodes.Status500InternalServerError, "Error adding exam");
        }

        [HttpPut("EditExam/{examId}")]
        public async Task<IActionResult> EditExam (int examId, [FromBody] ExamReq examReq)
        {
            if (examReq==null)
                return BadRequest("Exam data is required.");

            var userId = User.Claims.FirstOrDefault(c => c.Type=="Id")?.Value;
            var result = await examService.EditExsam(examId, examReq, userId);
            return result!=null ? Ok(result) : NotFound();
        }

        [HttpGet("{courseId}/Exams")]
        public async Task<IActionResult> GetAll (int courseId)
        {
            var exams = await examService.GetExams(courseId);
            return Ok(exams);
        }

        [HttpGet("GetExam/{id}")]
        public async Task<IActionResult> Get (int id)
        {
            var exam = await examService.GetByIdAsync(id);
            return exam==null ? NotFound() : Ok(exam);
        }

        [HttpDelete("DeleteExam/{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type=="Id")?.Value;
            await examService.DeleteAsync(id, userId);
            return NoContent();
        }

        // ================================
        // 🧩 QUESTION MANAGEMENT
        // ================================

        [HttpGet("ExamQuestions/{examId}")]
        public async Task<IActionResult> GetExamQuestions (int examId)
        {
            var questions = await questionService.GetAllQuestions(examId);
            return Ok(questions);
        }

        [HttpPost("AddQuestion/{examId}")]
        public async Task<IActionResult> AddQuestion (int examId, [FromBody] QuestionReq questionReq)
        {
            var userId = User.FindFirst("Id").Value;
            var result = await questionService.AddQuestion(questionReq, examId, userId);
            return result
                ? Ok("Question added successfully")
                : StatusCode(StatusCodes.Status500InternalServerError, "Error adding question");
        }

        [HttpPut("UpdateQuestion/{id}")]
        public async Task<IActionResult> UpdateQuestion (int id, [FromBody] UpdateQuestion questionReq)
        {
            var userId = User.FindFirst("Id").Value;
            var result = await questionService.UpdateQuestion(questionReq, id, userId);
            return result
                ? Ok("Question updated successfully")
                : StatusCode(StatusCodes.Status500InternalServerError, "Error updating question");
        }

        [HttpDelete("DeleteQuestion/{id}")]
        public async Task<IActionResult> DeleteQuestion (int id)
        {
            var userId = User.FindFirst("Id").Value;
            var result = await questionService.DeleteQuestion(id, userId);
            return result
                ? Ok("Question deleted successfully")
                : StatusCode(StatusCodes.Status500InternalServerError, "Error deleting question");
        }

        // ================================
        // 🔘 QUESTION OPTION MANAGEMENT
        // ================================

        [HttpPost("AddQuestionOption/{questionId}")]
        public async Task<IActionResult> AddQuestionOption (int questionId, [FromBody] QuestionOptionReq questionOptionReq)
        {
            var userId = User.FindFirst("Id").Value;
            var result = await questionService.AddQuestionOption(questionOptionReq, questionId, userId);
            return result
                ? Ok("Question option added successfully")
                : StatusCode(StatusCodes.Status500InternalServerError, "Error adding question option");
        }

        [HttpPut("UpdateQuestionOption/{id}")]
        public async Task<IActionResult> UpdateQuestionOption (int id, [FromBody] QuestionOptionReq questionOptionReq)
        {
            var userId = User.FindFirst("Id").Value;
            var result = await questionService.UpdateQuestionOption(questionOptionReq, id, userId);
            return result
                ? Ok("Question option updated successfully")
                : StatusCode(StatusCodes.Status500InternalServerError, "Error updating question option");
        }

        [HttpDelete("DeleteQuestionOption/{id}")]
        public async Task<IActionResult> DeleteQuestionOption (int id)
        {
            var userId = User.FindFirst("Id").Value;
            var result = await questionService.DeleteQuestionOption(id, userId);
            return result
                ? Ok("Question option deleted successfully")
                : StatusCode(StatusCodes.Status500InternalServerError, "Error deleting question option");
        }
        [HttpGet("ExsamWithReslutsDetals/{examId}")]
        public async Task<IActionResult> GetExamsWithResults (int examId)
        {
            var userId = User.FindFirst("Id").Value;
            var results = await examService.GetAllResultWithDetailsAsync(userId, examId);
            return results==null ? BadRequest("Results not found.") : Ok(results);
        }
        [HttpPatch("AddQustionPoints")]
        public async Task<IActionResult> AddQustionPoints ([FromBody] AddMarkReq markReq)
        {
            var userId = User.FindFirst("Id").Value;
            // public async Task<bool> addMarkToShortAns (AddMarkReq markReq, string userId)
            var result = await questionService.AddMarkToShortAns(markReq, userId);
            return result
                ? Ok("Points added successfully")
                : StatusCode(StatusCodes.Status500InternalServerError, "Error adding points");
        }
    }
}
