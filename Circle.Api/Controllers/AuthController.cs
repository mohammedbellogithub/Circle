using Circle.Core.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Circle.Api.Controllers
{
    /// <summary>
    /// OpenId Connect Auth Controller
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }



        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var errorLogs = await _userService.GetUsers();
                return Ok(errorLogs);

            }
            catch (Exception)
            {
                return BadRequest("Sorry, an error occurred while fetching user list, kindly contact support");
            }
        }
    }
}
