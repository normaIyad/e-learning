using Course.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Course.DAL.Repositry.Class
{
    public class UserRepositry : IUserRepositry
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepositry (UserManager<ApplicationUser> userManager)
        {
            this.userManager=userManager;
        }
        public async Task<bool> BlockUserAsync (string Id, int days)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user==null)
            {
                return false;
            }
            user.LockoutEnd=DateTimeOffset.UtcNow.AddDays(days);
            var result = await userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> ChangeUserRoleAsync (string Id, string role)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user==null)
            {
                return false;
            }
            var Userrole = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, Userrole);
            var result = await userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<List<ApplicationUser>> GetAllUsersAsync (Expression<Func<ApplicationUser, bool>>? expression = null, Expression<Func<ApplicationUser, object>>[] inclode = null, bool isTrck = true)
        {
            IQueryable<ApplicationUser> users = userManager.Users;
            if (expression!=null)
            {
                users=userManager.Users.Where(expression);
            }
            if (inclode!=null)
            {
                foreach (var item in inclode)
                {
                    users=users.Include(item);
                }
            }
            if (!isTrck)
            {
                users=users.AsNoTracking();
            }
            return await users.ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserAsync (string Id)
        {
            return await userManager.FindByIdAsync(Id);
        }

        public Task<bool> IsUserBlocked (string Id)
        {
            var user = userManager.FindByIdAsync(Id);
            if (user==null)
            {
                return Task.FromResult(false);
            }
            var isBlocked = user.Result.LockoutEnd.HasValue&&user.Result.LockoutEnd.Value>DateTimeOffset.UtcNow;
            return Task.FromResult(isBlocked);
        }

        public async Task<bool> UnBlockUserAsync (string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user==null)
            {
                return false;
            }
            user.LockoutEnd=null;
            var result = await userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
