using Course.Bll.Service.GenralIService;
using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;

namespace Course.Bll.Service.Interface
{
    public interface ICourseService : IGeneralService<CourseReq, CourseRes, Course.DAL.Models.Course>
    {
        Task<CourseWithMatirialResponce> GetCourseWithMaterialsAsync (int courseId);
        Task<bool> IsUserEnrollToCource (int courseId, string UserId);


    }
}
