using Circle.Core.Services.User;
using Circle.Core.ViewModels.User;
using Circle.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Circle.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserProfileController : BaseController
    {
        private readonly IUserService _userService;

        public UserProfileController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// View profile details
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-user-profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var response = await _userService.UserBioDetails();

            if (_userService.HasError)
            {
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);
            }


            return ApiResponse(response, "Successful");
        }

        [HttpPost("setup-user-profile")]
        public async Task<IActionResult> SetProfile([FromForm] SetUserProfileViewModel viewModel)
        {
            if (viewModel == null)
            {
                return EmptyPayloadResponse();
            }
            await _userService.SetProfile(viewModel);

            if (_userService.HasError)
            {
                return ApiResponse(null, _userService.Errors, ApiResponseCodes.ERROR);
            }


            return ApiResponse(null, "Profile Set up Successful");
        }
    }
}
