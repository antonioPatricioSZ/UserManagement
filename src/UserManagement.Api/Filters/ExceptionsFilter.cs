using System.Net;
using UserManagement.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserManagement.Communication.Response;
using UserManagement.Exceptions;

namespace UserManagement.Api.Filters;

public class ExceptionsFilter : IExceptionFilter {

    public void OnException(ExceptionContext context) {
        TratarUserManagementException(context);
    }

    private static void TratarUserManagementException(ExceptionContext context) {

        context.Result = (context.Exception) switch {

            ValidationErrorsException validationErrorsException => 
                context.Result = new BadRequestObjectResult(
                    new ErrorResponse(validationErrorsException.ErrorMessages)
                ),

            InvalidLoginException invalidLoginException => 
                context.Result = new UnauthorizedObjectResult(
                    new ErrorResponse(invalidLoginException.Message)
                ),

            NotFoundException notFoundException => 
                context.Result = context.Result = new NotFoundObjectResult(
                    new ErrorResponse(notFoundException.Message)
                ),

            ForbidenException forbidenException => 
                context.Result = context.Result = new ObjectResult(
                   new ErrorResponse(forbidenException.Message)
                ) { StatusCode = (int)HttpStatusCode.Forbidden },

            UnaunthorizedException unaunthorizedException => 
                context.Result = context.Result = new UnauthorizedObjectResult(
                    new ErrorResponse(unaunthorizedException.Message)
                ),

            _ => context.Result = new ObjectResult(
                    new ErrorResponse(ResourceErrorMessages.ERRO_DESCONHECIDO)
                ) { StatusCode = (int)HttpStatusCode.InternalServerError }

        };

        context.ExceptionHandled = true;

    }

}
