using Course.Bll.Service.GenralIService;
using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;
using Course.DAL.Repositry;
using Mapster;
using System.Linq;

namespace Course.Bll.Service.Class
{
    public class ExamService : GeneralService<ExamReq, ExamRes, Exam>, IExamService
    {
        private readonly IExamRepositry _examRepositry;
        private readonly ICourseRepositry _courseRepositry;
        private readonly IExamResultRepo _examResult;

        public ExamService (IGenralRepositry<Exam> repository, IExamRepositry examRepositry, ICourseRepositry courseRepositry, IExamResultRepo examResult)
            : base(repository)
        {
            _examRepositry=examRepositry;
            _courseRepositry=courseRepositry;
            _examResult=examResult;
        }

        public async Task<bool> AddExam (ExamReq examReq, int courseId, string instructorId)
        {
            var course = await _courseRepositry.GetByIdAsync(courseId);
            if (course==null) throw new Exception("Course Not Found");
            if (course.InstructorId!=instructorId)
                throw new Exception("You are not authorized to add exam to this course");

            var exam = examReq.Adapt<Exam>();
            exam.CourseId=courseId;
            await _examRepositry.AddAsync(exam);
            return true;
        }

        public async Task<bool> DeleteAsync (int id, string instructorId)
        {
            var exam = await _examRepositry.GetByIdAsync(id);
            if (exam==null) throw new Exception("Exam Not Found");

            var course = await _courseRepositry.GetByIdAsync(exam.CourseId);
            if (course?.InstructorId!=instructorId)
                throw new Exception("You are not authorized to delete exam of this course");

            await _examRepositry.DeleteAsync(exam);
            return true;
        }

        public async Task<ExamReq> EditExsam (int examId, ExamReq examReq, string instructorId)
        {
            var exam = await _examRepositry.GetByIdAsync(examId);
            if (exam==null) throw new Exception("Exam Not Found");

            var course = await _courseRepositry.GetByIdAsync(exam.CourseId);
            if (course?.InstructorId!=instructorId)
                throw new Exception("You are not authorized to edit exam of this course");

            exam.Title=examReq.Title;
            exam.Description=examReq.Description;
            exam.IsActive=examReq.IsActive;
            exam.Date=examReq.Date;
            exam.DurationInMinutes=examReq.DurationInMinutes;

            await _examRepositry.UpdateAsync(exam);
            return examReq;
        }

        public async Task<List<ExamReq>> GetExams (int courseId)
        {
            var exams = await _examRepositry.GetAllAsync(e => e.CourseId==courseId);
            if (exams==null||!exams.Any())
                throw new Exception("No Exams Found for the Given Course ID");

            return exams.Adapt<List<ExamReq>>();
        }

        public async Task<List<ExamResultReq>> GetExamsWithResults (int courseId, int examId, string userId)
        {
            var authorizedUser = await _courseRepositry.GetAllAsync(c => c.Id==courseId&&c.Enrollments.Any(e => e.UserId==userId));
            if (authorizedUser==null||!authorizedUser.Any())
                throw new Exception("You are not authorized to view exams of this course");

            var exams = await _examResult.GetAllAsync(e => e.Exam.CourseId==courseId&&e.ExamId==examId);
            var examResList = new List<ExamResultReq>();

            foreach (var exam in exams)
            {
                var examRes = exam.Adapt<ExamResultReq>();
                var result = await _examResult.GetAllAsync(er => er.ExamId==exam.Id&&er.UserId==userId);
                examRes.Score=result.FirstOrDefault()?.Score??0;
                examResList.Add(examRes);
            }

            return examResList;
        }

        public async Task<ExamReq> GetByIdAsync (int id)
        {
            var exam = await _examRepositry.GetByIdAsync(id);
            if (exam==null) throw new Exception("Exam Not Found");
            return exam.Adapt<ExamReq>();
        }

        public async Task<List<ExamResultWithDetails?>> GetAllResultWithDetailsAsync (string userId, int examId)
        {
            var authorizedInstructor = await _courseRepositry.GetAllAsync(c => c.InstructorId==userId);
            if (authorizedInstructor==null||!authorizedInstructor.Any())
                throw new Exception("You are not authorized to view exam results");

            return await _examResult.GetAllExamResultsWithDetailsAsync(examId);
        }

        public async Task<ExamStatisticsDto> ExamStatistics (int examId, string userId)
        {
            var authorizedInstructor = await _courseRepositry.GetAllAsync(c => c.InstructorId==userId&&c.Exams.Any(e => e.Id==examId));
            if (authorizedInstructor==null||!authorizedInstructor.Any())
                throw new Exception("You are not authorized to view exam statistics");

            var results = await _examResult.ExamStatistics(examId);
            if (results==null||!results.Any())
                throw new Exception("No statistics available for this exam");

            var dto = new ExamStatisticsDto
            {
                ExamTitle=results.First().Exam?.Title??"Unknown Exam",
                TotalStudents=results.Count,
                AverageScore=(double)results.Average(r => r.Score),
                HighestScore=(int)results.Max(r => r.Score),
                LowestScore=(int)results.Min(r => r.Score)
            };

            // Pass Rate
            dto.PassRate=results.Count(r => r.Score>=60)*100.0/dto.TotalStudents;

            // Score Distribution
            dto.ScoreDistribution=new List<ScoreRangeDto>
            {
                Range("90–100", results, 90, 100),
                Range("80–89", results, 80, 89),
                Range("70–79", results, 70, 79),
                Range("60–69", results, 60, 69),
                Range("<60", results, 0, 59)
            };

            // Most Missed Questions (null-safe)
            dto.MostMissedQuestions=results
                .SelectMany(r => r.Exam?.Questions??Enumerable.Empty<Question>())
                .GroupBy(q => q.QustionText)
                .Select(g => new MissedQuestionDto
                {
                    QuestionText=g.Key,
                    IncorrectPercentage=SafeCount(g)*100.0/SafeTotal(g)
                })
                .OrderByDescending(x => x.IncorrectPercentage)
                .Take(5)
                .ToList();

            return dto;
        }

        private ScoreRangeDto Range (string name, List<ExamResult> results, int min, int max)
        {
            var list = results.Where(r => r.Score>=min&&r.Score<=max).ToList();
            return new ScoreRangeDto
            {
                Range=name,
                Count=list.Count,
                Percentage=list.Count*100.0/results.Count
            };
        }

        // Null-safe helpers for MostMissedQuestions
        private double SafeCount (IGrouping<string, Question> g)
        {
            return g.SelectMany(q => q.QustionOptions??Enumerable.Empty<QuestionOption>())
                    .Count(o => o.IsCorrect&&
                                g.SelectMany(x => x.StudentAnswers??Enumerable.Empty<StudentAnswers>())
                                 .Any(sa => sa.QuestionOptionId==o.Id));
        }

        private double SafeTotal (IGrouping<string, Question> g)
        {
            return g.SelectMany(x => x.QustionOptions??Enumerable.Empty<QuestionOption>()).Count();
        }
    }
}
