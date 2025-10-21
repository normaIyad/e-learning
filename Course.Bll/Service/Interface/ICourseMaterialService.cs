using Course.Bll.Service.GenralIService;
using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;

namespace Course.Bll.Service.Interface
{
    public interface ICourseMaterialService : IGeneralService<CourseMaterialReq, CourseMaterialRes, CourseMaterial>
    {
        Task AddCourseMaterialAsync (CourseMaterialReq req, int courseId);
        Task<bool> DeleteAsync (int id);
        Task<bool> UpdateMaterialAsync (int id, CourseMaterialReq req);
        Task<bool> IsInstrctorCanAddMatirial (int courseId, string instructorId);
    }
}
