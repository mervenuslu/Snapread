using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SnapRead.Api.Model;
using SnapRead.Api.Model.Auth;
using SnapRead.Infrastructure.Data;

namespace SnapRead.Api.Controllers
{
  
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<BaseResponseModel> Register([FromBody] RegisterModel registerModel)
        {
            BaseResponseModel apiResponseModel = new BaseResponseModel();

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email,
                    CreatedAt = DateTime.Now
                };

                var userCreateResult = await _userManager.CreateAsync(user, registerModel.Password);
                if (userCreateResult.Succeeded)
                {
                    var emailConfirmtoken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = emailConfirmtoken }, this.Request.Scheme);
                    var message = new StringBuilder();
                    message.Append($"Please click <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>here</a> to confirm your email.");
                    await _emailSender.SendEmailAsync(user.Email, "Confirm Email", message.ToString());
                  
                    apiResponseModel.Message = "Confirmation email sent.";
                }
                else
                {

                    apiResponseModel.Errors.AddRange(userCreateResult.Errors.Select(x=>x.Description).ToList());
                }
            }
            else
            {

                apiResponseModel.Errors.Add("Model state is not valid.");
            }
            return apiResponseModel;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<object> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return false;
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return false;
            }
            return GenerateJwtToken(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<BaseResponseModel> SendConfirmationEmail(string email)
        {
            ApiResponseModel<object> apiResponseModel = new ApiResponseModel<object>();

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && user.EmailConfirmed == false)
            {
                var emailConfirmtoken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = emailConfirmtoken }, this.Request.Scheme);
                var message = new StringBuilder();
                message.Append($"Please click <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>here</a> to confirm your email.");
                await _emailSender.SendEmailAsync(user.Email, "Confirm Email", message.ToString());

                apiResponseModel.IsSuccess = true;
                apiResponseModel.Message = "Confirmation email is sent.";
            }
            else
            {
                apiResponseModel.Message = "Please check your credentials.";
                apiResponseModel.IsSuccess = false;
            }
            return apiResponseModel;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponseModel<string>> Login([FromBody] LoginModel model)
        {
            ApiResponseModel<string> apiResponseModel = new ApiResponseModel<string>();
            var user = await _userManager.FindByNameAsync(model.Login);
            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    apiResponseModel.Message = "You must have a confirmed email to log in.";
                    apiResponseModel.IsSuccess = false;
                }
                else
                {
                    var signinResult = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, false);
                    if (signinResult.Succeeded)
                    {
                        apiResponseModel.Message = "Success.";
                        apiResponseModel.IsSuccess = true;
                        apiResponseModel.Data = GenerateJwtToken(user);
                    }
                    else
                    {
                        apiResponseModel.Message = "Please check the credentials.";
                        apiResponseModel.IsSuccess = false;
                    }
                }
            }
            else
            {
                apiResponseModel.Message = "There is no user with this user id.";
                apiResponseModel.IsSuccess = false;
            }
            return apiResponseModel;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<BaseResponseModel> SendResetPasswordLink(string email)
        {
            BaseResponseModel apiResponseModel = new BaseResponseModel();
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var resetLink = Url.Action("ResetPassword",
                                "Account", new { userId = user.Id, token = token },
                                 protocol: HttpContext.Request.Scheme);

                apiResponseModel.Message = "Success.";
                apiResponseModel.IsSuccess = true;
            }
            else
            {
                apiResponseModel.Message = "There is no user with this user id or user id is not confirmed!";
                apiResponseModel.IsSuccess = false;
            }

            return apiResponseModel;
        }

        [HttpPost]
        public async Task<BaseResponseModel> ResetPassword(string userid, string token, string password)
        {
            BaseResponseModel apiResponseModel = new BaseResponseModel();
            var user = await _userManager.FindByIdAsync(userid);
            if (user != null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                var result = await _userManager.ResetPasswordAsync(user, token, password);
                if (result.Succeeded)
                {
                    apiResponseModel.Message = "Success.";
                    apiResponseModel.IsSuccess = true;
                }
                else
                {
                    apiResponseModel.Message = result.Errors.ToString();
                    apiResponseModel.IsSuccess = false;
                }

            }
            else
            {
                apiResponseModel.Message = "There is no user with this user id or user id is not confirmed!";
                apiResponseModel.IsSuccess = false;
            }
            return apiResponseModel;
        }
        private string GenerateJwtToken(IdentityUser user)
        {
            var utcNow = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, "SnapRead" + user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<String>("Jwt:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
                (
                signingCredentials: signingCredentials,
                claims: claims,
                expires: utcNow.AddDays(_configuration.GetValue<int>("Jwt:ExpireDays")),
                audience: _configuration.GetValue<String>("Jwt:Audience"),
                issuer: _configuration.GetValue<String>("Jwt:Issuer")
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
