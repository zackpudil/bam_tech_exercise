using Microsoft.AspNetCore.Mvc;
using StargateAPI.Models;
using System.Net;

namespace StargateAPI.Controllers
{
    public static class ControllerBaseExtensions
    {
        public static IActionResult GetResponse(this ControllerBase controllerBase, BaseResponse response, ILogger<ControllerBase> log)
        {
            log.Log(!response.Success ? LogLevel.Error : LogLevel.Information, response.Message);
            
            var httpResponse = new ObjectResult(response);
            httpResponse.StatusCode = response.ResponseCode;
            return httpResponse;
        }

        public static IActionResult GetResponse(this ControllerBase controllerBase, Exception ex, ILogger<ControllerBase> log)
        {
            return GetResponse(controllerBase, new BaseResponse
            {
                Message = ex.Message,
                Success = false,
                ResponseCode = ex is BadHttpRequestException ? ((BadHttpRequestException)ex).StatusCode : 500
            }, log);
        }
    }
}