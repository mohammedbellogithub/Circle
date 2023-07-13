using Circle.Shared.Enums;
using Circle.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Circle.Api.Controllers
{
    [Authorize(AuthenticationSchemes = AuthSchemes)]
    public abstract class BaseController : ControllerBase
    {
        private const string AuthSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult EmptyPayloadResponse()
        {
            return this.ApiResponse("", "Payload cannot be empty", ApiResponseCodes.INVALID_REQUEST);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ApiResponse(dynamic? data = default(object), string[]? messages = null,
          ApiResponseCodes codes = ApiResponseCodes.OK, int? totalCount = 0)
        {

            if (codes == ApiResponseCodes.OK)
            {
                var response = new ApiResponse<dynamic>
                {
                    Payload = data,
                    Code = codes,
                    Success = true,
                    Description = messages?.FirstOrDefault()
                };

                response.TotalCount = totalCount ?? 0;
                return ReturnHttpMessage(codes, response);
            }
            else
            {
                var response = new ApiResponse<dynamic>
                {
                    Payload = data,
                    Code = codes,
                    Errors = messages.ToList(),
                    Description = messages?.FirstOrDefault()
                };

                response.TotalCount = totalCount ?? 0;
                return ReturnHttpMessage(codes, response);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult ApiResponse(dynamic? data = default(object), string message = "", ApiResponseCodes codes = ApiResponseCodes.OK,
          int? totalCount = 0)
        {
            return ApiResponse(data, new string[] { message }, codes, totalCount);
        }


        private IActionResult ReturnHttpMessage<T>(ApiResponseCodes codes, ApiResponse<T> response) where T : class
        {
            switch (codes)
            {
                case ApiResponseCodes.EXCEPTION:
                    return this.StatusCode(StatusCodes.Status500InternalServerError, response);
                case ApiResponseCodes.UNAUTHORIZED:
                    return this.StatusCode(StatusCodes.Status401Unauthorized, response);
                case ApiResponseCodes.NOT_FOUND:
                case ApiResponseCodes.INVALID_REQUEST:
                case ApiResponseCodes.ERROR:
                    return this.StatusCode(StatusCodes.Status400BadRequest, response);
                case ApiResponseCodes.OK:
                default:
                    return Ok(response);
            }
        }
    }
}
