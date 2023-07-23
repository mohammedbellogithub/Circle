using Circle.Core.Services.Businesses;
using Circle.Core.ViewModels.Businesses;
using Circle.Core.ViewModels.User;
using Circle.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Circle.Api.Controllers
{
    [Route("api/[controller]")]
    public class BusinessCategoryController : BaseController
    {
        private readonly IBusinessCategoryService _businessCategoryService;

        public BusinessCategoryController(IBusinessCategoryService businessCategoryService)
        {
            _businessCategoryService = businessCategoryService;
        }
        /// <summary>
        /// Create business category endpoint
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("create-business-category")]
        public async Task<IActionResult> CreateBusinessCategory([FromForm] CreateBusinessCategoryViewModel viewModel)
        {
            if (viewModel == null)
            {
                return EmptyPayloadResponse();
            }
            await _businessCategoryService.CreateBusinessCategory(viewModel);

            if (_businessCategoryService.HasError)
            {
                return ApiResponse(null, _businessCategoryService.Errors, ApiResponseCodes.ERROR);
            }


            return ApiResponse(null, "Business Category successfully created.");
        }
        /// <summary>
        /// delete business category end point
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete-business-category")]
        public async Task<IActionResult> DeleteBusinessCategory(Guid id)
        {
            await _businessCategoryService.DeleteBusinessCategory(id);

            if (_businessCategoryService.HasError)
            {
                return ApiResponse(null, _businessCategoryService.Errors, ApiResponseCodes.ERROR);
            }


            return ApiResponse(null, "Business Category deleted successfully.");
        }
    }
}
