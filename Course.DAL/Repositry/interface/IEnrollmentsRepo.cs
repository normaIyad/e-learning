using Course.DAL.Models;

namespace Course.DAL.Repositry
{
    public interface IEnrollmentsRepo : IGenralRepositry<Enrollment>
    {
        Task<bool> IsUserEnrollToCource (int courseId, string UserId);
    }
}
