using Course.Bll.Service.Class;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Course.Pl.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Admin")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin , Admin , Instructor")]

    public class ReportController : ControllerBase
    {
        private readonly Repoer repoer;

        public ReportController (Repoer repoer)
        {
            this.repoer=repoer;
        }
        [HttpGet("ExsamReport/{id}")]
        public async Task<IActionResult> GenerateReport (int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type=="Id")?.Value;
            var document = await repoer.ExamResult(userId, id);
            // generate PDF file and return it as a response
            byte[] pdf = await repoer.ExamResult(userId, id);
            return File(pdf, "application/pdf", $"exsamRes{id}.pdf");
        }
        [HttpGet("ExamStatestic/{examId}")]
        public async Task<IActionResult> GetExamStatestic (int examId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type=="Id")?.Value;
            var statestic = await repoer.GenerateExamStatisticsReport(userId, examId);
            byte[] pdf = statestic;
            return File(pdf, "application/pdf", $"exsamRes.pdf");
        }
    }
}
