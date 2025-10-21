using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;

namespace Course.Bll.Service.Interface
{
    public interface IAuthentication
    {
        Task<string> GenerateToken (ApplicationUser user);
        Task<LogInRes> LogIn (LogInReq logInReq);
        Task<bool> Register (RegesterReq registerReq);
        Task<bool> ChangePassword (ChangePasswordReq changePasswordReq);
        Task<string> ConfirmEmail (string userId, string emailToken);
        Task<bool> ForgetPassword (string email);
        Task<bool> ResetPassword (string userId, string token, string newPassword);
    }
}
