﻿using Circle.Core.Services.Businesses;
using Circle.Core.ViewModels.Businesses;
using Circle.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Circle.Api.Controllers
{
    [Route("api/[controller]")]
    public class BusinessController : BaseController
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet("get-user-businesses")]
        public async Task<IActionResult> GetUserBusinesses()
        {

           var result =  await _businessService.GetUserBusinesses();

            if (_businessService.HasError)
            {
                return ApiResponse(null, _businessService.Errors, ApiResponseCodes.ERROR);
            }

            return ApiResponse(result, "Successful", ApiResponseCodes.OK);

        }

        [HttpPost("Create-business")]
        public async Task<IActionResult> CreateBusiness([FromBody] CreateBusinessViewModel viewModel)
        {
            if (viewModel == null)
            {
                return EmptyPayloadResponse();
            }

            await _businessService.CreateBusiness(viewModel);

            if (_businessService.HasError)
            {
                return ApiResponse(null, _businessService.Errors ,ApiResponseCodes.ERROR);
            }

            return ApiResponse(null , "Successful" , ApiResponseCodes.OK);

        }

        [HttpPut("update-business")]
        public async Task<IActionResult> UpdateBusiness([FromBody] EditBusinessViewModel viewModel)
        {
            if (viewModel == null)
            {
                return EmptyPayloadResponse();
            }

            await _businessService.UpdateBusiness(viewModel);

            if (_businessService.HasError)
            {
                return ApiResponse(null, _businessService.Errors, ApiResponseCodes.ERROR);
            }

            return ApiResponse(null, "Successful", ApiResponseCodes.OK);

        }
    }
}
