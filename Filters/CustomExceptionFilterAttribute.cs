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
        else if (context.Exception is Microsoft.WindowsAzure.Storage.StorageException)
        {
            context.Result = new ObjectResult(new { Message = context.Exception.Message })
            {
                StatusCode = 200
            };
        }
        else if (context.Exception is Amazon.S3.AmazonS3Exception)
        {
            context.Result = new ObjectResult(new { Message = context.Exception.Message })
            {
                StatusCode = 200
            };
        }
        else if (context.Exception is System.Net.WebException)
        {
            context.Result = new ObjectResult(new { Message = context.Exception.Message })
            {
                StatusCode = 200
            };
        }
        else if (context.Exception is Google.GoogleApiException)
        {
            context.Result = new ObjectResult(new { Message = context.Exception.Message })
            {
                StatusCode = 200
            };
        }
        else
        {
            context.Result = new ObjectResult(new { ErrorMessage = context.Exception.Message })
            {
                StatusCode = 500
            };
        }

        context.ExceptionHandled = true;
    }
}
