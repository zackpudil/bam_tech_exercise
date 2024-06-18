using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace StargateAPI.Controllers
{
    public static class ControllerBaseExtensions
    {

        public static IActionResult GetResponse(this ControllerBase controllerBase, BaseResponse response)
        {
            var httpResponse = new ObjectResult(response);
            httpResponse.StatusCode = response.ResponseCode;
            return httpResponse;
        }

        public static IActionResult GetResponse(this ControllerBase controllerBase, Exception ex)
        {
            return GetResponse(controllerBase, new BaseResponse
            {
                Message = ex.Message,
                Success = false,
                ResponseCode = ex is BadHttpRequestException ? ((BadHttpRequestException)ex).StatusCode : 500
            });
        }
    }
}