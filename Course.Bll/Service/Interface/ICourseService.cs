using Course.Bll.Service.GenralIService;
using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;

namespace Course.Bll.Service.Interface
{
    public interface ICourseService : IGeneralService<CourseReq, CourseRes, Course.DAL.Models.Course>
    {
        Task<CourseWithMatirialResponce> GetCourseWithMaterialsAsync (int courseId, string url);
        Task<bool> IsUserEnrollToCource (int courseId, string UserId);
        Task<List<CourseRes>> GetAllCourses (string url);
        Task <CourseRes> GetById (int id , string url);
        Task <int> addCourse(CourseReq courseReq);
        Task<int> removeCourse(int courseId);
        Task<int> updateCourse(int courseId, CourseReq courseReq);


    }
}
