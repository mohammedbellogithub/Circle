using Circle.Core.Services.User;
using Circle.Core.ViewModels.User;
using Circle.Shared.Enums;
using Circle.Shared.Models;
using Circle.Shared.Models.UserIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Circle.Api.Controllers
{
    
    public class UserManagementController : BaseController
    {
        private readonly IUserService _userService;
        private readonly UserManager<AppUsers> _userManager;

        public UserManagementController(IUserService userService, UserManager<AppUsers> userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/[controller]/user-list/{pageIndex}/{pageSize}")]
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

        [AllowAnonymous]
        [HttpPost()]
        [Route("api/[controller]/create-user")]
        public async Task<IActionResult> SignUp([FromBody] UserRegisterationViewModel viewModel)
        {
            if (!ModelState.IsValid) 
            {
                //return UnprocessableEntity(ModelState);
                return ApiResponse(UnprocessableEntity(ModelState), "TESTING", ApiResponseCodes.INVALID_REQUEST);
            };



            if (viewModel == null)
                return base.ApiResponse(default, "Empty payload", ApiResponseCodes.INVALID_REQUEST);

            await _userService.SignUp(viewModel);

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
    }
}
