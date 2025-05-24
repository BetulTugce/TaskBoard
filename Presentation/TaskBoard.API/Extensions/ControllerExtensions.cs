using Microsoft.AspNetCore.Mvc;
using TaskBoard.Application.Common;

namespace TaskBoard.API.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult MapErrorResult<T>(this ControllerBase controller, Result<T> result)
        {
            var errorResponse = new
            {
                message = result.Message,
                errorCode = result.ErrorCode.ToString()
            };

            return result.ErrorCode switch
            {
                ErrorCode.BadRequest => controller.BadRequest(errorResponse),
                ErrorCode.NotFound => controller.NotFound(errorResponse),
                ErrorCode.Unauthorized => controller.Unauthorized(errorResponse),
                ErrorCode.Forbidden => controller.StatusCode(StatusCodes.Status403Forbidden, errorResponse),
                ErrorCode.Conflict => controller.Conflict(errorResponse),
                ErrorCode.ValidationError => controller.UnprocessableEntity(errorResponse),
                ErrorCode.InternalError => controller.StatusCode(StatusCodes.Status500InternalServerError, errorResponse),
                _ => controller.BadRequest(errorResponse)
            };
        }
    }
}
