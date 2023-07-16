using Circle.Core.Components.Filter;
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
        /// Paginated user list
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="role"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [RequiresPermission(Permission.FULL_CONTROL)]
        [HttpGet("user-list/{pageIndex}/{pageSize}")]
        public async Task<IActionResult> GetUsers([FromQuery] string? keyword = null, [FromQuery] string? role = null, int? pageIndex = 1, int pageSize = 10)
        {
            var userList = await _userService.GetUsersAsync(keyword, role, pageIndex, pageSize);

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
        /// User account registration
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("signup-user")]
        public async Task<IActionResult> SignUp([FromBody] UserRegisterationViewModel viewModel)
        {
            

            if (viewModel == null)
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);

            var response = await _userService.SignUpAsync(viewModel);

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
        /// Add a user -- Aimed for Backoffice users
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [RequiresPermission(Permission.FULL_CONTROL)]
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUser([FromBody] UserRegisterationViewModel viewModel)
        {
            if (viewModel == null)
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);

            await _userService.AddUserAsync(viewModel);

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

        /// <summary>
        /// Validate OTP sent
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
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
        /// <summary>
        /// password change
        /// </summary>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Deactivate your account
        /// </summary>
        /// <returns></returns>
        [RequiresPermission(Permission.FULL_DEFAULT_USER_CONTROL)]
        [HttpPut("request-account-deactivation")]
        public async Task<IActionResult> SelfDeactivate()
        {
            await _userService.SelfDeactivateAsync();

            if (_userService.HasError)
            {
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);
            }
            return ApiResponse(null
            , "Account Deactivated successfully");

            //send mail
        }
        /// <summary>
        /// Deactivate user account 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [RequiresPermission(Permission.FULL_CONTROL)]
        [HttpPut("deactivate-user-account")]
        public async Task<IActionResult> DeactivateUser(string userId)
        {
            await _userService.DeactivateUserAsync(userId);

            if (_userService.HasError)
            {
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);
            }
            return ApiResponse(null
            , "Account Deactivated successfully");

            //send mail
        }
        /// <summary>
        ///  Delete user account
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [RequiresPermission(Permission.FULL_CONTROL)]
        [HttpDelete("delete-user-account")]
        public async Task<IActionResult> DeleteUserAccount(string userId)
        {
            await _userService.DeleteUserAccountAsync(userId);

            if (_userService.HasError)
            {
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);
            }
            return ApiResponse(null
            , "Account Deleted successfully");

            //send mail
        }

    }
}
