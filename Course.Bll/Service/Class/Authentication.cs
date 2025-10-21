using Course.Bll.Service.Interface;
using Course.DAL.DTO.request;
using Course.DAL.DTO.Responce;
using Course.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Course.Bll.Service.Class
{
    public class Authentication : IAuthentication
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager; // ✅ add back
        private readonly IConfiguration _configuration;
        public Authentication (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.userManager=userManager;
            this.signInManager=signInManager;
            _configuration=configuration;
        }
        public Task<string> ConfirmEmail (string userId, string emailToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ForgetPassword (string email)
        {
            //Areas
            throw new NotImplementedException();
        }

        public async Task<string> GenerateToken (ApplicationUser user)
        {
            var clans = new List<Claim>
            {
                new Claim("Name",user.UserName),
                new Claim("Email",user.Email),
                new Claim("Id",user.Id)
            };
            var roles = userManager.GetRolesAsync(user);
            foreach (var role in roles.Result)
            {
                clans.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT")["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                 claims: clans,
                 expires: DateTime.Now.AddMinutes(30),
                 signingCredentials: creds
                );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        public async Task<LogInRes> LogIn (LogInReq logInReq)
        {
            var user = await userManager.FindByEmailAsync(logInReq.Email);
            if (user!=null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, logInReq.Password, true);
                if (result.Succeeded)
                {
                    LogInRes logInRes = new LogInRes()
                    {
                        Token=await GenerateToken(user)
                    };
                    return logInRes;
                }
                if (result.IsLockedOut)
                {
                    return new LogInRes()
                    {
                        ErrorMessage="User is locked out"
                    };
                }
                if (result.IsNotAllowed)
                {
                    return new LogInRes()
                    {
                        ErrorMessage="User is not allowed to sign in"
                    };
                }
            }
            return new LogInRes()
            {
                ErrorMessage="Invalid Email or Password"
            };
        }

        public async Task<bool> Register (RegesterReq registerReq)
        {
            var user = new ApplicationUser
            {
                UserName=registerReq.UserName,
                Email=registerReq.Email,
                Address="",
                City="",
                Country="",
                DateOfBirth=null,
                FullName=""
            };
            var res = await userManager.CreateAsync(user, registerReq.Password);
            var roleRes = await userManager.AddToRoleAsync(user, Roles.User.ToString());


            return true;

        }
        public Task<bool> ResetPassword (string userId, string token, string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangePassword (ChangePasswordReq req)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user!=null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(user, req.OldPassword);
                if (!passwordCheck)
                {
                    return false;
                }
                var res = await userManager.ChangePasswordAsync(user, req.OldPassword, req.NewPassword);
                return res.Succeeded;
            }
            return false;
        }
    }
}
