using Course.DAL.Models;

namespace Course.DAL.Repositry
{
    public interface IExamRepositry : IGenralRepositry<Exam>
    {
        Task<Exam?> GetByIdWithCourseAsync (int examId);
        Task<decimal> GetTotalExsamResult (int examId);


    }
}
