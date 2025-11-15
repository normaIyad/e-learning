using Course.Bll.Service.GenralIService;
using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;
using Course.DAL.Repositry;
using Mapster;

namespace Course.Bll.Service.Class
{
    public class ExamService : GeneralService<ExamReq, ExamRes, Exam>, IExamService
    {
        private readonly IExamRepositry _examRepositry;
        private readonly ICourseRepositry courseRepositry;
        private readonly IExamResultRepo examResult;

        public ExamService (IGenralRepositry<Exam> repository, IExamRepositry examRepositry, ICourseRepositry courseRepositry, IExamResultRepo examResult) : base(repository)
        {
            _examRepositry=examRepositry;
            this.courseRepositry=courseRepositry;
            this.examResult=examResult;
        }

        public async Task<bool> AddExam (ExamReq examReq, int courseId, string InstactorId)
        {
            //  var exsistCourse = await _examRepositry.GetAllAsync(e => e.CourseId==courseId&&;
            var course = await courseRepositry.GetByIdAsync(courseId);
            var InstructorIdOfCourse = course?.InstructorId;
            if (InstructorIdOfCourse!=InstactorId)
            {
                throw new Exception("You are not authorized to add exam to this course");
            }
            if (course==null)
            {
                throw new Exception("Course Not Found");
            }

            var res = examReq.Adapt<Exam>();
            res.CourseId=courseId;
            await _examRepositry.AddAsync(res);
            return true;
        }

        public async Task<bool> DeleteAsync (int id, string InstactorId)
        {
            var existingExam = await _examRepositry.GetByIdAsync(id);
            if (existingExam==null)
            {
                throw new Exception("Exam Not Found");
            }
            var course = await courseRepositry.GetByIdAsync(existingExam.CourseId);
            var InstructorIdOfCourse = course?.InstructorId;
            if (InstructorIdOfCourse!=InstactorId)
            {
                throw new Exception("You are not authorized to delete exam of this course");
            }
            await _examRepositry.DeleteAsync(existingExam);
            return true;
        }

        public async Task<ExamReq> EditExsam (int examId, ExamReq examReq, string InstactorId)
        {
            var existingExam = await _examRepositry.GetByIdAsync(examId);
            if (existingExam==null)
                throw new Exception("Exam Not Found");

            var course = await courseRepositry.GetByIdAsync(existingExam.CourseId);
            if (course?.InstructorId!=InstactorId)
                throw new Exception("You are not authorized to edit exam of this course");

            // Update tracked entity directly
            existingExam.Title=examReq.Title;
            existingExam.Description=examReq.Description;
            existingExam.IsActive=examReq.IsActive;
            existingExam.Date=examReq.Date;
            existingExam.DurationInMinutes=examReq.DurationInMinutes;

            await _examRepositry.UpdateAsync(existingExam);
            return examReq;
        }

        public async Task<List<ExamReq>> GetExams (int CourseId)
        {
            var exams = await _examRepositry.GetAllAsync(e => e.CourseId==CourseId);
            var examReqs = exams.Adapt<List<ExamReq>>();
            return examReqs==null ? throw new Exception("No Exams Found for the Given Course ID") : examReqs;
        }
        public async Task<List<ExamResultReq>> GetExamsWithResults (int CourseId, int examId, string userId)
        {
            var outherizeUser = await courseRepositry.GetAllAsync(c => c.Id==CourseId&&c.Enrollments.Any(e => e.UserId==userId));
            if (outherizeUser==null||!outherizeUser.Any())
            {
                throw new Exception("You are not authorized to view exams of this course");
            }
            var exams = await examResult.GetAllAsync(e => e.Exam.CourseId==CourseId&&e.ExamId==examId);
            var examResList = new List<ExamResultReq>();
            foreach (var exam in exams)
            {
                var examRes = exam.Adapt<ExamResultReq>();
                var result = await examResult.GetAllAsync(er => er.ExamId==exam.Id&&er.UserId==userId);
                examRes.Score=result.FirstOrDefault()?.Score??0;
                examResList.Add(examRes);
            }
            return examResList;
        }
        public async Task<ExamReq> GetByIdAsync (int id)
        {
            var exam = await _examRepositry.GetByIdAsync(id);
            if (exam==null)
                throw new Exception("Exam Not Found");
            var examReq = exam.Adapt<ExamReq>();
            return examReq;
        }
        public async Task<List<ExamResultWithDetails?>> GetAllResultWithDetailsAsync (string userId, int examId)
        {
            var authrizeInstructor = await courseRepositry.GetAllAsync(c => c.InstructorId==userId);
            if (authrizeInstructor==null||!authrizeInstructor.Any())
            {
                throw new Exception("You are not authorized to view exam results");
            }
            var allResults = await examResult.GetAllExamResultsWithDetailsAsync(examId);
            return allResults;
        }



    }
}
