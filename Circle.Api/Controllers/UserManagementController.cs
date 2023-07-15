using Circle.Core.Services.Email;
using Circle.Core.Services.User;
using Circle.Core.ViewModels.User;
using Circle.Shared.Constants;
using Circle.Shared.Enums;
using Circle.Shared.Extensions;
using Circle.Shared.Models.UserIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Circle.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class UserManagementController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="emailService"></param>
        public UserManagementController(IUserService userService, IEmailService emailService)
        {
            _userService = userService;
            _emailService = emailService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="role"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("user-list/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetUsers([FromQuery] string? keyword = null, [FromQuery] string? role = null, int? pageIndex = 1, int pageSize = 10)
        {
            var userList = await _userService.GetUsers(keyword, role, pageIndex, pageSize);

            if (_userService.HasError)
            {
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);
            }

            return ApiResponse(userList.Select(u => new
            {
                u.Id,
                u.Username,
                u.Email,
                u.LastName,
                u.FirstName,
                u.MiddleName,
                u.PhoneNumber,
                u.IsActivated,
                u.Role,
                u.RoleId,
                u.IsDefaultPassword,
                u.Department,
                u.LastLoginDate,
                u.CreatedOn,
                u.ModifiedOn,
            }), Array.Empty<string>(), totalCount: userList.FirstOrDefault()?.TotalCount);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("signup-user")]
        public async Task<IActionResult> SignUp([FromBody] UserRegisterationViewModel viewModel)
        {
            

            if (viewModel == null)
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);

            var response = await _userService.SignUp(viewModel);

            if (_userService.HasError)
            {
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);
            }

            await Task.Factory.StartNew(async () =>
            {
                try
                {
                    var template = await _emailService.ReadTemplate(MessageTypes.VerificationMail);
                    var mailSubject = MessageSubject.VerificationMailSubject;
                    var logoUrl = $"{Request.Scheme}://{Request.Host}/images/applogo.jpg";

                    var messageToParse = new Dictionary<string, string>
                    {
                        { "{Name}", viewModel.Username},
                        { "{OTP}", response.Token},
                        { "{circle_logo}", logoUrl}
                    };
                    var message = template.ParseTemplate(messageToParse);
                    await _emailService.SendAsync(viewModel.Email, mailSubject, message);
                }
                catch (Exception ex)
                {

                    throw;
                }
            });

            return ApiResponse(new
            {
                viewModel.Username,
                viewModel.Email,
                viewModel.LastName,
                viewModel.FirstName,
                viewModel.MiddleName,
                viewModel.PhoneNumber,
                viewModel.Gender,
            }, "User created successfully");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] UserRegisterationViewModel viewModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    return ApiResponse(UnprocessableEntity(ModelState), "TESTING", ApiResponseCodes.INVALID_REQUEST);
            //};
            if (viewModel == null)
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);

            await _userService.AddUser(viewModel);

            if (_userService.HasError)
            {
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);
            }
            //send email to user for email confimation
            return ApiResponse(new
            {
                viewModel.Username,
                viewModel.Email,
                viewModel.LastName,
                viewModel.FirstName,
                viewModel.MiddleName,
                viewModel.PhoneNumber,
                viewModel.Gender,
            }, "User created successfully");
        }
        [AllowAnonymous]
        [HttpPost("validate-otp")]
        public async Task<IActionResult> ValidateOTP(string email, string code)
        {
            var result = await _userService.ValidateOTPAsync(email,code);

            if (_userService.HasError)
            {
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);
            }
            return ApiResponse( result
            , "OTP Verification successful");
        }
        [AllowAnonymous]
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendVerificationOTP(string email)
        {
            var response = await _userService.ResendOTPAsync(email);

            if (_userService.HasError)
            {
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);
            }


            await Task.Factory.StartNew(async () =>
            {
                var template = await _emailService.ReadTemplate(MessageTypes.VerificationMail);
                var mailSubject = MessageSubject.VerificationMailSubject;
                var logoUrl = $"{Request.Scheme}://{Request.Host}/images/applogo.jpg";

                var messageToParse = new Dictionary<string, string>
                    {
                        { "{Name}", response.UserName},
                        { "{OTP}", response.Token},
                        { "{circle_logo}", logoUrl}
                    };
                var message = template.ParseTemplate(messageToParse);
                await _emailService.SendAsync(email, mailSubject, message);
            });
            return ApiResponse(null
            , "OTP sent to mail");
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            var response = await _userService.ChangePasswordAsync(oldPassword, newPassword);

            if (_userService.HasError)
            {
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);
            }

            await Task.Factory.StartNew(async () =>
            {
                var template = await _emailService.ReadTemplate(MessageTypes.PasswordChange);
                var mailSubject = MessageSubject.PasswordchangeSubject;
                var logoUrl = $"{Request.Scheme}://{Request.Host}/images/applogo.jpg";

                var messageToParse = new Dictionary<string, string>
                    {
                        { "{Name}", response.UserName},
                        { "{circle_logo}", logoUrl}
                    };
                var message = template.ParseTemplate(messageToParse);
                await _emailService.SendAsync(response.Email, mailSubject, message);
            });
            return ApiResponse(null
            , "Password changed successfully");
        }

    }
}
