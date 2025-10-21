using Course.Bll.Service.GenralIService;
using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Repositry;
using Mapster;

namespace Course.Bll.Service.Class

{
    public class CourseService : GeneralService<CourseReq, CourseRes, Course.DAL.Models.Course>, ICourseService
    {
        private readonly IEnrollmentsRepo enrollments;
        private readonly ICourseRepositry courseRepositry;
        private readonly ICourseMaterialRepo materialRepo;

        public CourseService (IGenralRepositry<DAL.Models.Course> repository, IEnrollmentsRepo enrollments, ICourseRepositry courseRepositry, ICourseMaterialRepo materialRepo) : base(repository)
        {
            this.enrollments=enrollments;
            this.courseRepositry=courseRepositry;
            this.materialRepo=materialRepo;
        }
        public async Task<bool> IsUserEnrollToCource (int courseId, string UserId)
        {
            var isEnrolled = await enrollments.IsUserEnrollToCource(courseId, UserId);
            return isEnrolled;
        }

        public async Task<CourseWithMatirialResponce> GetCourseWithMaterialsAsync (int courseId)
        {
            var course = await courseRepositry.GetByIdAsync(courseId);
            if (course==null)
                return null!;
            var material = await materialRepo.GetAllAsync(e => e.CourseId==courseId);
            var courseWithMaterial = course.Adapt<CourseWithMatirialResponce>();
            courseWithMaterial.CourseMaterials=material.Adapt<ICollection<CourseMaterialRes>>();
            return courseWithMaterial;
        }
    }
}
