using ChatApp.Application.Exceptions;
using ChatApp.Domain.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.WebUtilities;

namespace ChatApp.Api.Middlewares;

public static class ExceptionMiddleware
{
    public static void HandleException(this WebApplication app) {
        app.UseExceptionHandler(config => 
            {
                config.Run(async httpContext => 
                {
                    httpContext.Response.ContentType = "application/json";
                    var errorFeatures = httpContext.Features.Get<IExceptionHandlerFeature>();
                    if (errorFeatures != null)
                    {
                        httpContext.Response.StatusCode = errorFeatures.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            UnauthorizedException => StatusCodes.Status401Unauthorized,
                            _ => StatusCodes.Status500InternalServerError
                        };

                        await httpContext.Response.WriteAsJsonAsync(new Error {
                                StatusCode = httpContext.Response.StatusCode,
                                Hint = ReasonPhrases.GetReasonPhrase(httpContext.Response.StatusCode),
                                Message = errorFeatures.Error.Message
                                });
                    }

                });
            });
    }
}
