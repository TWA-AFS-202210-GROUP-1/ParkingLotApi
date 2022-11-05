using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ParkingLotApi.Exceptions;

namespace ParkingLotApi.Filters;

public class HttpResponseExceptionFilter : IActionFilter
{

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is HttpResponseException httpResponseException)
        {
            context.Result = new ObjectResult(httpResponseException.ErrorMessage)
            {
                StatusCode = (int)httpResponseException.StatusCode
            };

            context.ExceptionHandled = true;
        }
    }
}