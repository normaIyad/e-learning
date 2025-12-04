using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Course.DAL.Models;
using Course.DAL.Repositry;
using System.Linq.Expressions;

namespace Course.Bll.Service.Class
{
    public class UserService : IUserService
    {
        private readonly IUserRepositry repositry;

        public UserService (IUserRepositry repositry)
        {
            this.repositry=repositry;
        }

        public Task<bool> BlockUserAsync (string Id, int days)
        {
            var result = repositry.BlockUserAsync(Id, days);
            return result;
        }

        public async Task<bool> ChangeUserRoleAsync (ChangeRoleReq req)
        {
            var result = await repositry.ChangeUserRoleAsync(req.UserId, req.Role);
            return result;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync (Expression<Func<ApplicationUser, bool>>? expression = null, Expression<Func<ApplicationUser, object>>[] inclode = null, bool isTrck = true)
        {
            return await repositry.GetAllUsersAsync(expression, inclode, isTrck);
        }

        public async Task<ApplicationUser?> GetUserAsync (string Id)
        {
            return await repositry.GetUserAsync(Id);
        }

        public async Task<bool> IsUserBlocked (string Id)
        {
            return await repositry.IsUserBlocked(Id);
        }

        public async Task<bool> UnBlockUserAsync (string Id)
        {
            return await repositry.UnBlockUserAsync(Id);
        }
    }
}
