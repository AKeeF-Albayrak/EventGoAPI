using EventGoAPI.Application.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EventGoAPI.API.Utilities
{
    public static class ResponseHandler
    {
        public static IActionResult CreateResponse<T>(T response) where T : class
        {
            if (response is null)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var responseTypeProperty = typeof(T).GetProperty("ResponseType");
            var messageProperty = typeof(T).GetProperty("Message");

            if (responseTypeProperty == null || messageProperty == null)
            {
                throw new ArgumentException("Response object does not contain required properties.");
            }

            var responseType = (ResponseType)responseTypeProperty.GetValue(response);
            var message = messageProperty.GetValue(response)?.ToString();

            return responseType switch
            {
                ResponseType.Success => new OkObjectResult(response),
                ResponseType.Unauthorized => new UnauthorizedObjectResult(new { message }),
                ResponseType.ValidationError => new BadRequestObjectResult(new { message }),
                ResponseType.Conflict => new ConflictObjectResult(new { message }),
                ResponseType.NotFound => new NotFoundObjectResult(new { message }),
                ResponseType.ServerError => new ObjectResult(new { message = "An internal server error occurred." })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                },
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
        }
    }

}
