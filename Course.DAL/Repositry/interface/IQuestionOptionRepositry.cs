using Course.DAL.Models;

namespace Course.DAL.Repositry
{
    public interface IQuestionOptionRepositry : IGenralRepositry<QuestionOption>
    {
        Task<bool> addOptions (List<QuestionOption> options);
        Task<bool> DeleteRangeAsync (IEnumerable<QuestionOption> options);
    }
}
