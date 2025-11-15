using Course.DAL.DataBase;
using Course.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Course.DAL.Repositry.Class
{
    public class QuestionRepositry : GenralRepositry<Question>, IQuestionRepositry
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepositry (ApplicationDbContext context) : base(context)
        {
            _context=context;
        }
        public async Task<Question?> GetQuestionWithOptionsAsync (int id)
        {
            return await _context.Questions
                .Include(q => q.QustionOptions)
                .FirstOrDefaultAsync(q => q.Id==id);
        }

        public async Task<bool> IsInstructorOfQuestionAsync (int questionId, string instructorId)
        {
           var IsAuthorized = await _context.Questions
                .AnyAsync(q => q.Id==questionId && q.Exam.Course.InstructorId==instructorId);
            return IsAuthorized;
        }
    }
}
