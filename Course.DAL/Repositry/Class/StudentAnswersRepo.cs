using Course.DAL.DataBase;
using Course.DAL.Models;

namespace Course.DAL.Repositry.Class
{
    public class StudentAnswersRepo : GenralRepositry<StudentAnswers>, IStudentAnswersRepo
    {
        private readonly ApplicationDbContext context;

        public StudentAnswersRepo (ApplicationDbContext context) : base(context)
        {
            this.context=context;
        }
        public async Task SubmitAnswersAsync (List<StudentAnswers> req)
        {
            await context.AddRangeAsync(req);
            await context.SaveChangesAsync();
        }
    }
}
