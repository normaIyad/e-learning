using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;
using Course.DAL.Repositry;

namespace Course.Bll.Service.Class
{
    public class StudentAnswersService : IStudentAnswersService
    {
        private readonly IStudentAnswersRepo _answersRepo;
        private readonly IExamRepositry _examRepo;
        private readonly IQuestionRepositry _questionRepo;
        private readonly IQuestionOptionRepositry _optionRepo;
        private readonly IExamResultRepo examResult;

        public StudentAnswersService (
            IStudentAnswersRepo answersRepo,
            IExamRepositry examRepo,
            IQuestionRepositry questionRepo,
            IQuestionOptionRepositry optionRepo,
            IExamResultRepo examResult)
        {
            _answersRepo=answersRepo;
            _examRepo=examRepo;
            _questionRepo=questionRepo;
            _optionRepo=optionRepo;
            this.examResult=examResult;
        }


        public async Task<bool> SubmitAnswersAsync (ExamSubmissionReq req, string userId, int examId)
        {
            var authorized = await _examRepo.GetAllAsync(e =>
                e.Id==examId&&
                e.Course.Enrollments.Any(en => en.UserId==userId));
            if (!authorized.Any())
                throw new Exception("You are not authorized to submit answers for this exam.");
            if (req.Answers==null||!req.Answers.Any())
                throw new Exception("Answers cannot be empty.");
            var existingAnswers = await _answersRepo.GetAllAsync(a =>
                a.Question.ExamId==examId&&a.userId==userId);
            if (existingAnswers.Any())
                throw new Exception("You have already submitted this exam.");
            var studentAnswers = new List<StudentAnswers>();
            decimal SumPoints = 0;

            foreach (var answer in req.Answers)
            {
                var question = await _questionRepo.GetQuestionWithOptionsAsync(answer.QuestionId);
                if (question==null||question.ExamId!=examId)
                    throw new Exception($"Invalid question ID {answer.QuestionId}.");
                var studentAnswer = new StudentAnswers
                {
                    QuestionId=answer.QuestionId,
                    userId=userId,
                };
                if (question.QustionType==QustionType.MultipleChoice||question.QustionType==QustionType.TrueFalse)
                {
                    if (!answer.QuestionOptionId.HasValue)
                        throw new Exception($"Question {answer.QuestionId} requires an option selection.");

                    var selectedOption = await _optionRepo.GetByIdAsync(answer.QuestionOptionId.Value);
                    if (selectedOption==null||selectedOption.QuestionId!=answer.QuestionId)
                        throw new Exception($"Invalid option for question {answer.QuestionId}.");

                    studentAnswer.QuestionOptionId=selectedOption.Id;
                    studentAnswer.IsCorrect=selectedOption.IsCorrect;
                    studentAnswer.PointsEarned=selectedOption.IsCorrect ? question.Points : 0;
                    SumPoints+=studentAnswer.PointsEarned;
                }
                else if (question.QustionType==QustionType.ShortAnswer)
                {
                    if (string.IsNullOrWhiteSpace(answer.AnswerText))
                        throw new Exception($"Question {answer.QuestionId} requires an answer text.");

                    studentAnswer.AnswerText=answer.AnswerText;
                    studentAnswer.IsCorrect=false;
                    studentAnswer.PointsEarned=0;
                }
                studentAnswers.Add(studentAnswer);
            }
            await _answersRepo.SubmitAnswersAsync(studentAnswers);
            await examResult.AddAsync(new DAL.Models.ExamResult
            {
                ExamId=examId,
                UserId=userId,
                Score=SumPoints
            });

            return true;
        }


        public async Task<decimal> GetResultAsync (int examId, string studentId)
        {
            var IfThereRes = await examResult.GetAllAsync(er =>
                er.ExamId==examId&&er.UserId==studentId);
            if (!IfThereRes.Any())
            {
                var exsam = await _examRepo.GetByIdAsync(examId);
                if (exsam==null)
                    throw new Exception("Exam Not Found");
                var userAnswers = await _answersRepo.GetAllAsync(sa =>
                    sa.Question.ExamId==examId&&sa.userId==studentId);
                decimal totalScore = 0;
                foreach (var answer in userAnswers)
                {
                    totalScore+=answer.PointsEarned;
                }
                var examResults = new ExamResult
                {
                    ExamId=examId,
                    UserId=studentId,
                    Score=totalScore,
                    DateTaken=DateTime.UtcNow
                };
                var addRes = await examResult.AddExamResultAsync(examResults);
                return totalScore;
            }
            var getResult = await examResult.GetExamResultAsync(examId, studentId);
            return getResult==null ? throw new Exception("No answers found for this exam.") : getResult.Score;
        }
        public async Task<ExamResultWithDetails?> GetResultWithDetailsAsync (int examId, string studentId)
        {
            var resultWithDetails = await examResult.GetExamResultWithDetailsAsync(examId, studentId);
            return resultWithDetails;
        }
    }
}
