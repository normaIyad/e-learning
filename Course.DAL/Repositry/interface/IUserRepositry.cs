using Course.DAL.Models;
using System.Linq.Expressions;

namespace Course.DAL.Repositry
{
    public interface IUserRepositry
    {
        Task<bool> BlockUserAsync (string Id, int days);
        Task<bool> UnBlockUserAsync (string Id);
        Task<bool> IsUserBlocked (string Id);
        Task<bool> ChangeUserRoleAsync (string Id, string role);
        Task<ApplicationUser?> GetUserAsync (string Id);
        Task<List<ApplicationUser>> GetAllUsersAsync (Expression<Func<ApplicationUser, bool>>? expression = null, Expression<Func<ApplicationUser, object>>[] inclode = null, bool isTrck = true);
    }
}
