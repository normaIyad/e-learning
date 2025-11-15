using Course.DAL.DataBase;
using Course.DAL.Models;

namespace Course.DAL.Repositry.Class
{
    public class QuestionOptionRepositry : GenralRepositry<QuestionOption>, IQuestionOptionRepositry
    {
        private readonly ApplicationDbContext context;

        public QuestionOptionRepositry (ApplicationDbContext context) : base(context)
        {
            this.context=context;
        }

        public async Task<bool> addOptions (List<QuestionOption> options)
        {
            await context.QuestionOptions.AddRangeAsync(options);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRangeAsync (IEnumerable<QuestionOption> options)
        {
            context.QuestionOptions.RemoveRange(options);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
