using Course.Bll.Service.Interface;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Course.Bll.Service.Class
{
    public class Repoer
    {
        private readonly IExamService _examService;

        public Repoer (IExamService examService)
        {
            _examService=examService;
        }

        public async Task<byte[]> ExamResult (string userId, int examId)
        {
            var examReport = await _examService.GetAllResultWithDetailsAsync(userId, examId);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // Header
                    page.Header()
                        .Text($"Exam Report for User: {userId}")
                        .SemiBold()
                        .FontSize(18);

                    // Content
                    page.Content().Column(col =>
                    {
                        foreach (var exam in examReport)
                        {
                            col.Item().Text($"Exam Title: {exam.ExamTitle}").Bold().FontSize(14);

                            foreach (var q in exam.ExamQuestion)
                            {
                                col.Item().Text($"Question: {q.QuestionText}");

                                foreach (var op in q.Options)
                                {
                                    var selected = op.IsSelected ? "(Selected)" : "";
                                    var correct = op.IsCorrect ? "(Correct)" : "";
                                    col.Item().Text($"- {op.OptionText} {selected} {correct}");
                                }

                                col.Item().Text(""); // empty line after question
                            }

                            col.Item().Text(""); // empty line after exam
                        }
                    });

                    // Footer
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }

        public async Task<byte[]> GenerateExamStatisticsReport (string userId, int examId)
        {
            var stats = await _examService.ExamStatistics(examId, userId);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    // Header
                    page.Header().Text("Exam Statistics Report").SemiBold().FontSize(18);
                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Exam: {stats.ExamTitle}");
                        col.Item().Text($"Total Students: {stats.TotalStudents}");
                        col.Item().Text($"Average Score: {stats.AverageScore:F1}%");
                        col.Item().Text($"Highest Score: {stats.HighestScore}%");
                        col.Item().Text($"Lowest Score: {stats.LowestScore}%");
                        col.Item().Text($"Pass Rate: {stats.PassRate:F1}%");

                        col.Item().Text(""); // empty line
                        col.Item().Text("Score Distribution:");
                        foreach (var range in stats.ScoreDistribution)
                        {
                            col.Item().Text($"{range.Range}: {range.Count} students ({range.Percentage:F1}%)");
                        }

                        if (stats.MostMissedQuestions!=null&&stats.MostMissedQuestions.Any())
                        {
                            col.Item().Text(""); // empty line
                            col.Item().Text("Most Missed Questions:");
                            int i = 1;
                            foreach (var q in stats.MostMissedQuestions)
                            {
                                col.Item().Text($"{i}. {q.QuestionText} - {q.IncorrectAttempts} of {q.TotalAttempts} students answered incorrectly ({q.IncorrectPercentage:F1}%)");
                                i++;
                            }
                        }
                    });

                    // Footer
                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}
