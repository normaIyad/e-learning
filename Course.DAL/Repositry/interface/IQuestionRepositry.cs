using Course.DAL.Models;

namespace Course.DAL.Repositry
{
    public interface IQuestionRepositry : IGenralRepositry<Question>
    {
        Task<Question?> GetQuestionWithOptionsAsync (int id);
        Task<bool> IsInstructorOfQuestionAsync (int questionId, string instructorId);
        Task<bool> AddManyQustions (List<Question> questions);




    }
}
