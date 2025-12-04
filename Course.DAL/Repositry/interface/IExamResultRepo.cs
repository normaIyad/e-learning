using Course.DAL.DTO.Responce;
using Course.DAL.Models;

namespace Course.DAL.Repositry
{
    public interface IExamResultRepo : IGenralRepositry<ExamResult>
    {
        Task<ExamResult?> GetExamResultAsync (int examId, string studentId);
        Task<ExamResultWithDetails?> GetExamResultWithDetailsAsync (int examId, string studentId);
        Task<List<ExamResultWithDetails?>> GetAllExamResultsWithDetailsAsync (int examId);
        Task<bool> AddExamResultAsync (ExamResult examResult);
        Task<bool> UpdateExamResultAsync (ExamResult examResult);
        Task<List<ExamResult>> ExamStatistics (int exsamId);


    }
}
