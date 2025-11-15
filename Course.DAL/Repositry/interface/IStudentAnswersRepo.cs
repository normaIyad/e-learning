using Course.DAL.Models;

namespace Course.DAL.Repositry
{
    public interface IStudentAnswersRepo : IGenralRepositry<StudentAnswers>
    {
        Task SubmitAnswersAsync (List<StudentAnswers> req);
    }
}
