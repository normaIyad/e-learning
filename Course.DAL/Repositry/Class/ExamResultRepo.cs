using Course.DAL.DataBase;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Course.DAL.Repositry.Class
{
    public class ExamResultRepo : GenralRepositry<ExamResult>, IExamResultRepo
    {
        private readonly ApplicationDbContext context;

        public ExamResultRepo (ApplicationDbContext context) : base(context)
        {
            this.context=context;
        }

        public async Task<bool> AddExamResultAsync (ExamResult examResult)
        {
            await context.ExamResults.AddAsync(examResult);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ExamResultWithDetails?>> GetAllExamResultsWithDetailsAsync (int examId)
        {
            var exam = await context.Exams
                .Include(e => e.Questions)
                    .ThenInclude(q => q.QustionOptions)
                .FirstOrDefaultAsync(e => e.Id==examId);

            if (exam==null)
                throw new Exception("Exam Not Found");

            var allResults = await context.ExamResults
                .Include(er => er.Exam)
                    .ThenInclude(e => e.Questions)
                        .ThenInclude(q => q.QustionOptions)
                .Where(er => er.ExamId==examId)
                .ToListAsync();

            // For each result, fetch the student's selected answers
            var examResultsWithDetails = new List<ExamResultWithDetails?>();

            foreach (var result in allResults)
            {
                var studentAnswers = await context.StudentAnswers
                    .Where(sa => sa.userId==result.UserId&&sa.Question.ExamId==examId)
                    .ToListAsync();

                var examResultWithDetails = new ExamResultWithDetails
                {
                    ExamId=result.ExamId,
                    ExamTitle=result.Exam.Title,
                    Score=result.Score,
                    DateTaken=result.DateTaken,
                    userId=result.UserId,
                    ExamQuestion=new List<ExsamQustionWithAnsRes>(),

                };

                foreach (var question in result.Exam.Questions)
                {
                    var questionWithAns = new ExsamQustionWithAnsRes
                    {
                        Id=question.Id,
                        QuestionText=question.QustionText,

                        Options=new List<QuestionOptionIsSelectedRes>()
                    };
                    if (question.QustionType==QustionType.ShortAnswer)
                    {
                        var studentAnswer = studentAnswers
                            .FirstOrDefault(sa => sa.QuestionId==question.Id);
                        questionWithAns.AnswerText=studentAnswer?.AnswerText;
                    }
                    foreach (var option in question.QustionOptions)
                    {
                        var optionRes = new QuestionOptionIsSelectedRes
                        {
                            Id=option.Id,
                            OptionText=option.OptionText,
                            IsCorrect=option.IsCorrect,
                            IsSelected=studentAnswers.Any(sa => sa.QuestionId==question.Id&&sa.QuestionOptionId==option.Id)
                        };
                        questionWithAns.Options.Add(optionRes);
                    }

                    examResultWithDetails.ExamQuestion.Add(questionWithAns);

                }

                examResultsWithDetails.Add(examResultWithDetails);
            }

            return examResultsWithDetails;
        }

        public async Task<ExamResult?> GetExamResultAsync (int examId, string studentId)
        {
            return await context.ExamResults
                .FirstOrDefaultAsync(e => e.ExamId==examId&&e.UserId==studentId);
        }

        public async Task<ExamResultWithDetails?> GetExamResultWithDetailsAsync (int examId, string studentId)
        {
            var res = await context.ExamResults
                .Include(er => er.Exam)
                    .ThenInclude(e => e.Questions)
                        .ThenInclude(q => q.QustionOptions)
                .Where(er => er.ExamId==examId&&er.UserId==studentId)
                .FirstOrDefaultAsync();

            if (res==null)
                return null;

            var studentAnswers = await context.StudentAnswers
                .Where(sa => sa.userId==studentId&&sa.Question.ExamId==examId)
                .ToListAsync();

            var examResultWithDetails = new ExamResultWithDetails
            {
                ExamId=res.ExamId,
                ExamTitle=res.Exam.Title,
                Score=res.Score,
                UserName=(await context.Users.FindAsync(res.UserId))?.UserName,
                DateTaken=res.DateTaken,
                userId=res.UserId,
                ExamQuestion=new List<ExsamQustionWithAnsRes>()
            };

            foreach (var question in res.Exam.Questions)
            {
                var questionWithAns = new ExsamQustionWithAnsRes
                {
                    Id=question.Id,
                    QuestionText=question.QustionText,
                    Options=new List<QuestionOptionIsSelectedRes>()
                };

                foreach (var option in question.QustionOptions)
                {
                    var optionRes = new QuestionOptionIsSelectedRes
                    {
                        Id=option.Id,
                        OptionText=option.OptionText,
                        IsCorrect=option.IsCorrect,
                        IsSelected=studentAnswers.Any(sa => sa.QuestionId==question.Id&&sa.QuestionOptionId==option.Id)
                    };
                    questionWithAns.Options.Add(optionRes);
                }

                examResultWithDetails.ExamQuestion.Add(questionWithAns);
            }

            return examResultWithDetails;
        }
        public async Task<bool> UpdateExamResultAsync (ExamResult examResult)
        {
            context.ExamResults.Update(examResult);
            await context.SaveChangesAsync();
            return true;
        }
        public async Task<List<ExamResult>> ExamStatistics (int exsamId)
        {
            var res = await context.ExamResults.Where(e => e.ExamId==exsamId).Include(e => e.User).Include(e => e.Exam).Include(e => e.Exam.Questions).ThenInclude(q => q.QustionOptions).ToListAsync();
            return res;
        }
    }
}
