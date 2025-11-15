using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;

namespace Course.Bll.Service.Interface
{
    public interface IAuthentication
    {
        Task<string> GenerateToken (ApplicationUser user);
        Task<LogInRes> LogIn (LogInReq logInReq, string host);
        Task<bool> Register (RegesterReq registerReq, string host);
        Task<bool> ChangePassword (ChangePasswordReq changePasswordReq);
        Task<string> ConfirmEmail (string userId, string emailToken);
        Task<bool> ForgetPassword (ForgetPasswordReq req);
        Task<bool> ResetPassword (PasswordRestReq req);

    }
}
