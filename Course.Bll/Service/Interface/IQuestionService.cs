using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Repositry.Class;

namespace Course.Bll.Service.Interface
{
    public interface IQuestionService
    {
        Task<QuestionRes> GetQuestion (int id);
        Task<List<QuestionRes>> GetAllQuestions (int examId);
        Task<bool> AddQuestion (QuestionReq questionReq, int examId, string userId);
        Task<bool> AddManyQuestions (List<QuestionReq> questionReqs, int examId, string userId);
        Task<bool> UpdateQuestion (UpdateQuestion questionReq, int id, string userId);
        Task<bool> DeleteQuestion (int id, string userId);
        Task<bool> AddQuestionOption (QuestionOptionReq questionOptionReq, int questionId, string userId);
        Task<bool> UpdateQuestionOption (QuestionOptionReq questionOptionReq, int id, string userId);
        Task<bool> DeleteQuestionOption (int id, string userId);
        Task<List<QuestionRes>> ExamQuestions (int examId, string userId);
        Task<bool> AddMarkToShortAns (AddMarkReq markReq, string userId);


    }
}
