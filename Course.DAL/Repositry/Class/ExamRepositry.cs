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


    }
}
