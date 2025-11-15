using Course.Bll.Service.GenralIService;
using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;

namespace Course.Bll.Service.Interface
{
    public interface IExamService : IGeneralService<ExamReq, ExamRes, Exam>
    {
        Task<bool> AddExam (ExamReq examReq, int courseId, string InstactorId);
        Task<ExamReq> EditExsam (int examId, ExamReq examReq, string InstactorId);
        Task<List<ExamReq>> GetExams (int CourseId);
        Task<bool> DeleteAsync (int id, string InstactorId);
        Task<List<ExamResultWithDetails?>> GetAllResultWithDetailsAsync (string userId, int examId);





    }
}
