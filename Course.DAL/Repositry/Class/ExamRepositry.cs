using Course.DAL.DataBase;
using Course.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Course.DAL.Repositry.Class
{
    public class ExamRepositry : GenralRepositry<Exam>, IExamRepositry
    {
        private readonly ApplicationDbContext context;

        public ExamRepositry (ApplicationDbContext context) : base(context)
        {
            this.context=context;
        }
        public async Task<Exam?> GetByIdWithCourseAsync (int examId)
        {
            return await context.Exams
                .Include(e => e.Course)
                .FirstOrDefaultAsync(e => e.Id==examId);
        }

        public async Task<decimal> GetTotalExsamResult (int examId)
        {
            var totalPoints = await context.Questions
                .Where(q => q.ExamId==examId)
                .SumAsync(q => (decimal)q.Points);

            return totalPoints;
        }
    }
}
