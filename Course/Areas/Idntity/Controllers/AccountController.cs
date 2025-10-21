using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Microsoft.AspNetCore.Mvc;

namespace Course.Pl.Areas.Idntity.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Idntity")]

    public class AccountController : ControllerBase
    {
        private readonly IAuthentication _authentication;

        public AccountController (IAuthentication authentication)
        {
            _authentication=authentication;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LogIn ([FromBody] LogInReq logInReq)
        {
            var result = await _authentication.LogIn(logInReq);
            return result==null ? Unauthorized() : Ok(result);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register ([FromBody] RegesterReq registerReq)
        {
            var result = await _authentication.Register(registerReq);
            return !result ? BadRequest() : Ok();
        }
        [HttpPatch("ChngePassword")]
        public async Task<IActionResult> ChangePassword ([FromBody] ChangePasswordReq changePasswordReq)
        {
            var result = await _authentication.ChangePassword(changePasswordReq);
            return !result ? BadRequest() : Ok();
        }

    }
}
