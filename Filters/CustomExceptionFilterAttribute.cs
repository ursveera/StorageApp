using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

public class CustomExceptionFilterAttribute : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ApplicationException)
        {
            context.Result = new BadRequestObjectResult(new { ErrorMessage = context.Exception.Message });
        }
        else
        {
            context.Result = new ObjectResult(new { ErrorMessage = "An error occurred." })
            {
                StatusCode = 500
            };
        }

        context.ExceptionHandled = true;
    }
}
