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
        private readonly IFileService fileService;

        public CourseService (IGenralRepositry<DAL.Models.Course> repository, IEnrollmentsRepo enrollments, ICourseRepositry courseRepositry, ICourseMaterialRepo materialRepo, IFileService fileService) : base(repository)
        {
            this.enrollments=enrollments;
            this.courseRepositry=courseRepositry;
            this.materialRepo=materialRepo;
            this.fileService=fileService;
        }
        public async Task<bool> IsUserEnrollToCource (int courseId, string UserId)
        {
            var isEnrolled = await enrollments.IsUserEnrollToCource(courseId, UserId);
            return isEnrolled;
        }

        public async Task<CourseWithMatirialResponce> GetCourseWithMaterialsAsync (int courseId, string url)
        {
            var course = await courseRepositry.GetByIdAsync(courseId);
            if (course==null)
                return null!;
            var material = await materialRepo.GetAllAsync(e => e.CourseId==courseId);
            var courseWithMaterial = course.Adapt<CourseWithMatirialResponce>();
            courseWithMaterial.FileUrl=$"{url}/CourseMaterials/{course.ImgeUrl}";
            courseWithMaterial.CourseMaterials=material.Adapt<ICollection<CourseMaterialRes>>();
            return courseWithMaterial;
        }




        async Task<List<CourseRes>> ICourseService.GetAllCourses (string url)
        {
            var course = await courseRepositry.GetAllAsync();
            var courseRes = course.Adapt<List<CourseRes>>();
            foreach (var item in courseRes)
            {
                item.ImgeUrl=$"{url}CourseImgs/{item.ImgeUrl}";
            }
            return courseRes;
        }

        async Task<CourseRes> ICourseService.GetById (int id, string url)
        {
            var course = await courseRepositry.GetByIdAsync(id);
            if (course==null)
                return null!;
            var courseRes = course.Adapt<CourseRes>();
            courseRes.ImgeUrl=$"{url}CourseImgs/{courseRes.ImgeUrl}";
            return courseRes;

        }

        async Task<int> ICourseService.addCourse (CourseReq courseReq)
        {
            var course = courseReq.Adapt<DAL.Models.Course>();
            if (course.ImgeUrl!=null)
            {
                var addImg = await fileService.UploadFileAsync(courseReq.ImgeUrl, "Imgs/CourseImgs");
                course.ImgeUrl=addImg;
            }
            await courseRepositry.AddAsync(course);
            return 1;
        }

        async Task<int> ICourseService.removeCourse (int courseId)
        {
            var course = await courseRepositry.GetByIdAsync(courseId);
            if (course==null)
                throw new Exception("Course Not Found");
            await fileService.DeleteFileAsync(course.ImgeUrl, "Imgs/CourseImgs");
            await courseRepositry.DeleteAsync(course);
            return 1;
        }

        async Task<int> ICourseService.updateCourse (int courseId, CourseReq courseReq)
        {
            var course = await courseRepositry.GetByIdAsync(courseId);
            if (course==null)
                throw new Exception("Course Not Found");
            courseReq.Adapt(course);
            if (courseReq.ImgeUrl!=null)
            {
                await fileService.DeleteFileAsync(course.ImgeUrl, "CourseImgs");
                var addImg = await fileService.UploadFileAsync(courseReq.ImgeUrl, "CourseImgs");
                course.ImgeUrl=addImg;
            }
            await courseRepositry.UpdateAsync(course);
            return 1;
        }
    }
}
