using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Microsoft.AspNetCore.Mvc;

namespace Course.Pl.Areas.Identity.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Identity")]
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
            var request = $"{Request.Scheme}://{Request.Host}";
            var result = await _authentication.LogIn(logInReq, request);
            return result==null ? Unauthorized() : Ok(result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register ([FromBody] RegesterReq registerReq)
        {
            var request = $"{Request.Scheme}://{Request.Host}";
            var result = await _authentication.Register(registerReq, request);
            return !result ? BadRequest() : Ok();
        }

        [HttpPatch("ChangePassword")]
        public async Task<IActionResult> ChangePassword ([FromBody] ChangePasswordReq changePasswordReq)
        {
            var result = await _authentication.ChangePassword(changePasswordReq);
            return !result ? BadRequest() : Ok();
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail ([FromQuery] string userId, [FromQuery] string token)
        {
            var result = await _authentication.ConfirmEmail(userId, token);
            return result==null ? BadRequest() : Ok(result);
        }
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword ([FromBody] ForgetPasswordReq req)
        {
            var result = await _authentication.ForgetPassword(req);
            return !result ? BadRequest() : Ok();
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword ([FromBody] PasswordRestReq req)
        {
            var result = await _authentication.ResetPassword(req);
            return !result ? BadRequest() : Ok();
        }
    }
}
