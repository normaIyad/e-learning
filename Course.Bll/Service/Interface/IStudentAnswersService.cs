using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;

namespace Course.Bll.Service.Interface
{
    public interface IStudentAnswersService
    {
        Task<bool> SubmitAnswersAsync (ExamSubmissionReq req, string userId, int examId);
        Task<decimal> GetResultAsync (int examId, string studentId);
        Task<ExamResultWithDetails?> GetResultWithDetailsAsync (int examId, string studentId);




    }
}
