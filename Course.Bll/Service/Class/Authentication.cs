using Course.Bll.Service.GenralIService;
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
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        public Authentication (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, IEmailSender emailSender)
        {
            this.userManager=userManager;
            this.signInManager=signInManager;
            _configuration=configuration;
            _emailSender=emailSender;
        }
        public async Task<string> ConfirmEmail (string userId, string emailToken)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user==null)
            {
                throw new Exception("User not found");
            }
            if (user.EmailConfirmed)
            {
                return "Email is already confirmed";
            }
            var res = userManager.ConfirmEmailAsync(user, emailToken);
            return res.Result.Succeeded ? "Email confirmed successfully" : "Error confirming email";

        }

        public async Task<bool> ForgetPassword (ForgetPasswordReq req)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user==null)
            {
                throw new Exception("User not found");
            }
            var randommCode = new Random();
            var code = randommCode.Next(100000, 999999).ToString();
            user.PasswordRestCode=code;
            user.PasswordRestCodeExpiration=DateTime.Now.AddMinutes(15);
            await userManager.UpdateAsync(user);
            var htmlBody = $@"<!DOCTYPE html>
             <html lang=""en""> 
         <head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Password Reset</title>
    <style>
        /* Reset CSS */
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}

        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            line-height: 1.6;
            color: #333;
            background-color: #f6f6f6;
            padding: 20px;
        }}

        .email-container {{
            max-width: 600px;
            margin: 0 auto;
            background: #ffffff;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }}

        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 40px 30px;
            text-align: center;
            color: white;
        }}

        .header h1 {{
            font-size: 28px;
            font-weight: 600;
            margin-bottom: 10px;
        }}

        .header p {{
            font-size: 16px;
            opacity: 0.9;
        }}

        .content {{
            padding: 40px 30px;
        }}

        .code-container {{
            text-align: center;
            margin: 30px 0;
        }}

        .verification-code {{
            display: inline-block;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            font-size: 32px;
            font-weight: bold;
            letter-spacing: 8px;
            padding: 20px 40px;
            border-radius: 8px;
            margin: 20px 0;
            font-family: 'Courier New', monospace;
        }}

        .instructions {{
            background: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            border-left: 4px solid #667eea;
            margin: 25px 0;
        }}

        .instructions h3 {{
            color: #667eea;
            margin-bottom: 10px;
        }}

        .instructions ul {{
            list-style-position: inside;
            margin-left: 10px;
        }}

        .instructions li {{
            margin-bottom: 8px;
        }}

        .button-container {{
            text-align: center;
            margin: 30px 0;
        }}

        .reset-button {{
            display: inline-block;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            text-decoration: none;
            padding: 15px 40px;
            border-radius: 6px;
            font-weight: 600;
            font-size: 16px;
            transition: transform 0.2s ease;
        }}

        .reset-button:hover {{
            transform: translateY(-2px);
        }}

        .footer {{
            background: #f8f9fa;
            padding: 25px 30px;
            text-align: center;
            border-top: 1px solid #e9ecef;
            color: #6c757d;
            font-size: 14px;
        }}

        .warning {{
            background: #fff3cd;
            border: 1px solid #ffeaa7;
            color: #856404;
            padding: 15px;
            border-radius: 6px;
            margin: 20px 0;
            text-align: center;
        }}

        .support {{
            margin-top: 25px;
            padding-top: 25px;
            border-top: 1px solid #e9ecef;
        }}

        .support a {{
            color: #667eea;
            text-decoration: none;
        }}

        /* Responsive Design */
        @media only screen and (max-width: 600px) {{
            .content {{
                padding: 30px 20px;
            }}
            
            .header {{
                padding: 30px 20px;
            }}
            
            .verification-code {{
                font-size: 24px;
                padding: 15px 30px;
                letter-spacing: 6px;
            }}
            
            .reset-button {{
                padding: 12px 30px;
                font-size: 14px;
            }}
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <!-- Header -->
        <div class=""header"">
            <h1>Password Reset</h1>
            <p>You requested to reset your password</p>
        </div>

        <!-- Content -->
        <div class=""content"">
            <p>Hello {user.UserName},</p>
            
            <p>We received a request to reset your password for your E-Larning account. Use the verification code below to reset your password:</p>

            <!-- Verification Code -->
            <div class=""code-container"">
                <div class=""verification-code"" id=""verificationCode"">
                    {code}
                </div>
            </div>

            <!-- Instructions -->
            <div class=""instructions"">
                <h3>📝 Instructions:</h3>
                <ul>
                    <li>Enter this code on the password reset page</li>
                    <li>The code will expire in 15 minutes</li>
                    <li>If you didn't request this, please ignore this email</li>
                    <li>For security reasons, don't share this code with anyone</li>
                </ul>
            </div>
            <!-- Security Warning -->
            <div class=""warning"">
                ⚠️ <strong>Security Tip:</strong> Never share your verification code with anyone. Our team will never ask for this code.
            </div>
            <!-- Support Section -->
            <div class=""support"">
                <p>Need help? <a href=""mailto:support@yourapp.com"">Contact our support team</a></p>
                <p>Or visit our <a href=""[HELP_LINK]"">help center</a></p>
            </div>
        </div>

        <!-- Footer -->
        <div class=""footer"">
            <p>© 2024 E-Larning. All rights reserved.</p>
            <p>Jerusamen Abu-Dis</p>
            <p>
                <a href=""[UNSUBSCRIBE_LINK]"" style=""color: #6c757d;"">Unsubscribe</a> | 
                <a href=""[PRIVACY_LINK]"" style=""color: #6c757d;"">Privacy Policy</a>
            </p>
        </div>
    </div>
</body>
</html>";
            await _emailSender.SendEmailAsync(user.Email, "Password Reset Code", htmlBody);
            return true;
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

        public async Task<LogInRes> LogIn (LogInReq logInReq, string host)
        {
            var user = await userManager.FindByEmailAsync(logInReq.Email);
            if (user!=null)
            {
                var result = await signInManager.CheckPasswordSignInAsync(user, logInReq.Password, true);
                if (result.Succeeded)
                {
                    if (!user.EmailConfirmed)
                    {
                        return new LogInRes()
                        {
                            ErrorMessage="Email is not confirmed"
                        };
                    }
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

        public async Task<bool> Register (RegesterReq registerReq, string host)
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

            // ✅ Stop if user creation failed
            if (!res.Succeeded)
            {
                foreach (var error in res.Errors)
                {
                    Console.WriteLine($"❌ Error: {error.Description}");
                }
                return false;
            }

            // ✅ Add role AFTER confirming success
            var roleRes = await userManager.AddToRoleAsync(user, Roles.User.ToString());
            if (!roleRes.Succeeded)
            {
                foreach (var error in roleRes.Errors)
                {
                    Console.WriteLine($"❌ Role Error: {error.Description}");
                }
                return false;
            }

            // ✅ Generate token & correct URL
            var emailToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"{host}/api/Identity/Account/ConfirmEmail?userId={user.Id}&token={Uri.EscapeDataString(emailToken)}";

            //  Send HTML email with proper interpolation
            var htmlBody = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{ font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f7f9fc; margin: 0; padding: 20px; line-height: 1.6; color: #333; }}
        .email-container {{ max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 16px; overflow: hidden; box-shadow: 0 8px 24px rgba(0, 0, 0, 0.08); }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 50px 30px; text-align: center; color: white; }}
        .header h1 {{ font-size: 32px; font-weight: 700; margin-bottom: 10px; }}
        .header p {{ font-size: 18px; opacity: 0.95; }}
        .content {{ padding: 50px 40px; text-align: center; }}
        .welcome-text {{ font-size: 18px; color: #555; margin-bottom: 30px; }}
        .highlight {{ color: #667eea; font-weight: 600; }}
        .button-container {{ margin: 40px 0; }}
        .confirm-button {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white !important; text-decoration: none; padding: 18px 45px; border-radius: 12px; font-size: 18px; font-weight: 600; transition: all 0.3s ease; box-shadow: 0 4px 15px rgba(102, 126, 234, 0.3); }}
        .confirm-button:hover {{ transform: translateY(-2px); box-shadow: 0 6px 20px rgba(102, 126, 234, 0.4); }}
        .alternative-link {{ background: #f8f9fa; padding: 20px; border-radius: 10px; margin: 25px 0; border-left: 4px solid #667eea; }}
        .link-text {{ word-break: break-all; font-family: 'Courier New', monospace; font-size: 13px; color: #667eea; background: #f1f3f9; padding: 12px; border-radius: 6px; border: 1px dashed #667eea; }}
        .info-box {{ background: #e8f4fd; border: 1px solid #b6e0fe; border-radius: 10px; padding: 20px; margin: 30px 0; text-align: left; }}
        .footer {{ background: #f8f9fa; padding: 30px; text-align: center; border-top: 1px solid #e9ecef; color: #6c757d; }}
        @media only screen and (max-width: 600px) {{
            .content {{ padding: 40px 25px; }}
            .header {{ padding: 40px 25px; }}
            .header h1 {{ font-size: 28px; }}
            .confirm-button {{ padding: 16px 35px; font-size: 16px; }}
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='header'>
            <h1>Welcome to E-Larning! 🎉</h1>
            <p>Confirm your email address to get started</p>
        </div>
        <div class='content'>
            <p class='welcome-text'>Hello <span class='highlight'>{user.UserName}</span>,<br>Thank you for signing up! Please confirm your email address to activate your account.</p>
            <div class='button-container'>
                <a href='{confirmationLink}' class='confirm-button'>Confirm Email Address</a>
            </div>
            <div class='alternative-link'>
                <p><strong>If the button doesn't work, copy and paste this link into your browser:</strong></p>
                <div class='link-text'>{confirmationLink}</div>
            </div>
            <div class='info-box'>
                <h3>📧 Why confirm your email?</h3>
                <ul>
                    <li>Verify your identity and secure your account</li>
                    <li>Receive important notifications and updates</li>
                    <li>Reset your password if you forget it</li>
                    <li>Access all features of [App Name]</li>
                </ul>
            </div>
            <p style='color: #666; font-size: 14px;'>This confirmation link will expire in 24 hours for security reasons.</p>
        </div>
        <div class='footer'>
            <p>If you didn't create an account with [App Name], please ignore this email.</p>
            <p>Need help? <a href='mailto:support@yourapp.com' style='color: #667eea; text-decoration: none;'>Contact our support team</a></p>
            <div style='margin-top: 15px;'>
                <a href='[Privacy Policy]' style='color: #6c757d; text-decoration: none; margin: 0 10px; font-size: 13px;'>Privacy Policy</a>
                <a href='[Terms of Service]' style='color: #6c757d; text-decoration: none; margin: 0 10px; font-size: 13px;'>Terms of Service</a>
            </div>
            <p style='margin-top: 20px; font-size: 12px; color: #999;'>© 2024 [Your Company Name]. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

            await _emailSender.SendEmailAsync(user.Email, "Confirm Your Email", htmlBody);

            return true;
        }

        public async Task<bool> ResetPassword (PasswordRestReq req)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user is null)
            {
                throw new Exception("User not found");
            }
            if (user.PasswordRestCode!=req.PasswordRestCode||user.PasswordRestCodeExpiration<DateTime.Now)
            {
                throw new Exception("Invalid or expired password reset code");
            }
            var res = await userManager.ResetPasswordAsync(user, await userManager.GeneratePasswordResetTokenAsync(user), req.NewPassword);
            if (res.Succeeded)
            {
                user.PasswordRestCode=null;
                user.PasswordRestCodeExpiration=null;
                await userManager.UpdateAsync(user);
                return true;
            }
            else
            {
                var errors = string.Join(", ", res.Errors.Select(e => e.Description));
                throw new Exception($"Password reset failed: {errors}");
            }
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
