using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;
using Course.DAL.Repositry;
using Course.DAL.Repositry.Class;
using Mapster;

namespace Course.Bll.Service.Class
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepositry _questionRepo;
        private readonly IQuestionOptionRepositry _optionRepo;
        private readonly IExamResultRepo _examResult;
        private readonly IExamRepositry _examRepo;
        private readonly IStudentAnswersRepo _strudentAns;
        public QuestionService (IQuestionRepositry questionRepo, IQuestionOptionRepositry optionRepo, IExamResultRepo examResult, IExamRepositry examRepo, IStudentAnswersRepo strudentAns)
        {
            _questionRepo=questionRepo;
            _optionRepo=optionRepo;
            _examResult=examResult;
            _examRepo=examRepo;
            _strudentAns=strudentAns;
        }

        private async Task CheckQuestionAuthorization (int questionId, string userId)
        {
            var isAuthorized = await _questionRepo.IsInstructorOfQuestionAsync(questionId, userId);

            if (!isAuthorized)
            {
                var question = await _questionRepo.GetByIdAsync(questionId);
                if (question==null)
                {
                    throw new Exception("Question not found."); // Or throw a dedicated NotFoundException
                }

                throw new UnauthorizedAccessException("You are not authorized to modify this resource.");
            }
        }
        public async Task<bool> AddQuestion (QuestionReq questionReq, int examId, string userId)
        {
            var isAuthorized = await _examRepo.GetAllAsync(e => e.Id==examId&&e.Course.InstructorId==userId);
            if (isAuthorized==null||!isAuthorized.Any())
                throw new UnauthorizedAccessException("You are not authorized to add questions to this exam.");
            var exsistExsam = await _examRepo.GetByIdAsync(examId);
            if (exsistExsam==null)
                throw new Exception("Exam not found");
            var question = questionReq.Adapt<Question>();
            question.ExamId=examId;
            if (question.QustionType==QustionType.ShortAnswer&&
               questionReq.QustionOptions!=null&&questionReq.QustionOptions.Any())
                throw new InvalidOperationException("Short Answer questions cannot have options.");
            await _questionRepo.AddAsync(question);
            if (questionReq.QustionOptions!=null&&questionReq.QustionOptions.Any())
            {
                if (question.QustionType==QustionType.TrueFalse&&questionReq.QustionOptions.Count!=2)
                    throw new InvalidOperationException("True/False questions must have exactly two options.");
                if (question.QustionType==QustionType.TrueFalse&&
                    !questionReq.QustionOptions.Any(o => o.OptionText.Equals("true", StringComparison.OrdinalIgnoreCase))||
                    !questionReq.QustionOptions.Any(o => o.OptionText.Equals("false", StringComparison.OrdinalIgnoreCase)))
                    throw new InvalidOperationException("For True/False questions, options must be 'True' and 'False'.");
                if (question.QustionType!=QustionType.TrueFalse&&
                    questionReq.QustionOptions.Count>=4)
                    throw new InvalidOperationException("Cannot have more than 5 options for this question type.");
                if (questionReq.QustionOptions.Count(o => o.IsCorrect)>1)
                    throw new InvalidOperationException("Only one correct option is allowed per question.");
                var options = questionReq.QustionOptions.Adapt<List<QuestionOption>>();
                foreach (var option in options)
                {
                    option.QuestionId=question.Id;
                }
                await _optionRepo.addOptions(options);
            }
            return true;

        }
        public async Task<bool> AddManyQuestions (List<QuestionReq> questionReqs, int examId, string userId)
        {
            var isAuthorized = await _examRepo.GetAllAsync(e => e.Id==examId&&e.Course.InstructorId==userId);
            if (isAuthorized==null||!isAuthorized.Any())
                throw new UnauthorizedAccessException("You are not authorized to add questions to this exam.");
            var exsistExsam = await _examRepo.GetByIdAsync(examId);
            if (exsistExsam==null)
                throw new Exception("Exam not found");
            var questions = questionReqs.Adapt<List<Question>>();
            foreach (var question in questions)
            {
                question.ExamId=examId;
            }
            await _questionRepo.AddManyQustions(questions);
            return true;
        }

        public async Task<bool> AddQuestionOption (QuestionOptionReq questionOptionReq, int questionId, string userId)
        {
            if (questionOptionReq==null)
                throw new ArgumentNullException(nameof(questionOptionReq));
            await CheckQuestionAuthorization(questionId, userId);
            var question = await _questionRepo.GetQuestionWithOptionsAsync(questionId);
            if (question==null)
                throw new Exception("Question not found");
            if (question.QustionType==QustionType.ShortAnswer)
                throw new InvalidOperationException("Cannot add options to Short Answer questions");
            if (question.QustionOptions!=null&&question.QustionOptions.Count>=4)
                throw new InvalidOperationException("Cannot add more than 4 options to a question");
            if (question.QustionType==QustionType.TrueFalse&&
                !questionOptionReq.OptionText.Equals("true", StringComparison.OrdinalIgnoreCase)&&
                !questionOptionReq.OptionText.Equals("false", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("For True/False questions, option text must be 'True' or 'False'");

            if (question.QustionOptions!=null&&questionOptionReq.IsCorrect&&
                question.QustionOptions.Any(o => o.IsCorrect))
                throw new InvalidOperationException("Only one correct option is allowed per question");

            var option = questionOptionReq.Adapt<QuestionOption>();
            option.QuestionId=questionId;

            await _optionRepo.AddAsync(option);
            return true;
        }

        public async Task<bool> DeleteQuestion (int id, string userId)
        {
            await CheckQuestionAuthorization(id, userId);
            var question = await _questionRepo.GetByIdAsync(id);
            var deletedOptions = await _optionRepo.GetAllAsync(o => o.QuestionId==question.Id);
            if (deletedOptions.Any())
            {
                await _optionRepo.DeleteRangeAsync(deletedOptions);
            }

            await _questionRepo.DeleteAsync(question);
            return true;
        }

        public async Task<bool> DeleteQuestionOption (int id, string userId)
        {
            var option = await _optionRepo.GetByIdAsync(id);
            if (option==null)
                throw new Exception("Question option not found");
            var authorized = await _optionRepo.GetAllAsync(o =>
                 o.Id==id&&o.Question.Exam.Course.InstructorId==userId); // Assuming 'Question' navigation property is available

            if (authorized==null||!authorized.Any())
                throw new UnauthorizedAccessException("You are not authorized to delete this question option");

            await _optionRepo.DeleteAsync(option);
            return true;
        }

        public async Task<List<QuestionRes>> GetAllQuestions (int examId)
        {
            var questions = await _questionRepo.GetAllAsync(q => q.ExamId==examId);
            if (questions==null||!questions.Any())
                throw new Exception("No questions found for the given exam ID");
            var questionIds = questions.Select(q => q.Id).ToList();
            var options = await _optionRepo.GetAllAsync(o => questionIds.Contains(o.QuestionId));
            var result = questions.Adapt<List<QuestionRes>>();
            foreach (var question in result)
            {
                if (options==null||!options.Any())
                    continue;
                question.QustionOptions=options
                    .Where(o => o.QuestionId==question.Id)
                    .Adapt<List<QuestionOptionRes>>();
            }

            return result;
        }
        public async Task<List<QuestionRes>> ExamQuestions (int examId, string userId)
        {
            var questions = await _questionRepo.GetAllAsync(q => q.ExamId==examId);
            if (questions==null||!questions.Any())
                throw new Exception("No questions found for the given exam ID");
            var questionIds = questions.Select(q => q.Id).ToList();
            var options = await _optionRepo.GetAllAsync(o => questionIds.Contains(o.QuestionId));
            var result = questions.Adapt<List<QuestionRes>>();
            foreach (var question in result)
            {
                if (options==null||!options.Any())
                    continue;
                question.QustionOptions=options
              .Where(o => o.QuestionId==question.Id)
              .Select(o => new QuestionOptionRes
              {
                  Id=o.Id,
                  OptionText=o.OptionText,
                  IsCorrect=false // Hide correct answer
              })
              .ToList();

            }

            return result;
        }
        public async Task<QuestionRes> GetQuestion (int id)
        {
            var question = await _questionRepo.GetByIdAsync(id);
            return question==null ? throw new Exception("Question not found") : question.Adapt<QuestionRes>();
        }
        public async Task<bool> UpdateQuestion (UpdateQuestion questionReq, int id, string userId)
        {
            var question = await _questionRepo.GetByIdAsync(id);
            if (question==null)
                throw new Exception("Question not found");
            await CheckQuestionAuthorization(id, userId);
            questionReq.Adapt(question);
            var updated = await _questionRepo.UpdateAsync(question);
            return updated>0;
        }

        public async Task<bool> UpdateQuestionOption (QuestionOptionReq questionOptionReq, int id, string userId)
        {
            var option = await _optionRepo.GetByIdAsync(id);
            if (option==null)
                throw new Exception("Question option not found");
            var authorized = await _optionRepo.GetAllAsync(o =>
                o.Id==id&&o.Question.Exam.Course.InstructorId==userId);
            if (authorized==null||!authorized.Any())
                throw new UnauthorizedAccessException("You are not authorized to update this question option");
            questionOptionReq.Adapt(option);
            var updated = await _optionRepo.UpdateAsync(option);
            return updated>0;
        }
        public async Task<bool> AddMarkToShortAns (AddMarkReq markReq, string userId)
        {
            // 1️⃣ Authorization check — instructor must own the question
            var authorized = await _questionRepo.GetAllAsync(q =>
                q.Id==markReq.QuestionId&&q.Exam.Course.InstructorId==userId);
            if (authorized==null||!authorized.Any())
                throw new UnauthorizedAccessException("You are not authorized to add marks to this question.");

            // 2️⃣ Validate the question
            var question = await _questionRepo.GetByIdAsync(markReq.QuestionId);
            if (question==null)
                throw new Exception("Question not found");

            if (question.QustionType!=QustionType.ShortAnswer)
                throw new InvalidOperationException("Marks can only be added to Short Answer questions");

            if (markReq.Mark<0)
                throw new InvalidOperationException("Mark cannot be negative");

            if (question.Points<markReq.Mark)
                throw new InvalidOperationException("Mark exceeds the maximum points for this question");

            if (string.IsNullOrEmpty(markReq.studentId))
                throw new ArgumentNullException("studentId cannot be null or empty");

            // 3️⃣ Get the student’s exam result
            var getExamResult = await _examResult.GetExamResultAsync(question.ExamId, markReq.studentId);
            if (getExamResult==null)
                throw new Exception("Exam result not found for the student");

            // 4️⃣ Get the student's answer to this question
            var studentAnswer = await _strudentAns.GetAllAsync(sa =>
                sa.QuestionId==markReq.QuestionId&&sa.userId==markReq.studentId);
            if (studentAnswer==null||!studentAnswer.Any())
                throw new Exception("Student answer not found for the given question and student");

            var answerToUpdate = studentAnswer.First();

            // 5️⃣ Handle mark updates
            var oldMark = answerToUpdate.PointsEarned;
            var scoreDifference = markReq.Mark-oldMark;

            // update PointsEarned in StudentAnswers
            answerToUpdate.PointsEarned=markReq.Mark;
            answerToUpdate.IsCorrect=markReq.Mark>0; // optional: mark as correct if earned > 0

            await _strudentAns.UpdateAsync(answerToUpdate);

            // 6️⃣ Update total exam score accordingly
            getExamResult.Score+=scoreDifference;
            await _examResult.UpdateAsync(getExamResult);

            return true;
        }


    }
}