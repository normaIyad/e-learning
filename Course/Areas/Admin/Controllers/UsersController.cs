using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Course.Pl.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Admin")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin, Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService service;

        public UsersController (IUserService service)
        {
            this.service=service;
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers ()
        {
            var users = await service.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser (string id)
        {
            var user = await service.GetUserAsync(id);
            return user==null ? NotFound() : Ok(user);
        }
        [HttpPost("BlockUser/{id}/{days}")]
        public async Task<IActionResult> BlockUser (string id, int days)
        {
            var result = await service.BlockUserAsync(id, days);
            return !result ? BadRequest("Unable to block user.") : Ok("User blocked successfully.");
        }
        [HttpPost("UnBlockUser/{id}")]
        public async Task<IActionResult> UnBlockUser (string id)
        {
            var result = await service.UnBlockUserAsync(id);
            return !result ? BadRequest("Unable to unblock user.") : Ok("User unblocked successfully.");
        }
        [HttpGet("IsUserBlocked/{id}")]
        public async Task<IActionResult> IsUserBlocked (string id)
        {
            var isBlocked = await service.IsUserBlocked(id);
            return Ok(isBlocked);
        }
        [HttpPost("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole ([FromBody] ChangeRoleReq req)
        {
            var result = await service.ChangeUserRoleAsync(req);
            return !result ? BadRequest("Unable to change user role.") : Ok("User role changed successfully.");
        }


    }
}
