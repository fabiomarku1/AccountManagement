using System;
using System.Net;
using AccountManagement.ErrorHandling.HandlerException;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AccountManagement.ErrorHandling
{
    public static class ExceptionMiddlewareExtensions
    {
    //    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    //    {
    //        app.UseExceptionHandler(error =>
    //        {
    //            error.Run(async context =>
    //            {
    //                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    //                context.Response.ContentType = "application/json";

    //                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

    //                if (contextFeature != null)
    //                {
    //                    // Log
    //                    await context.Response.WriteAsync(new ErrorModel()
    //                    {
    //                        StatusCode = context.Response.StatusCode,
    //                        Message = "Internal server error,"


    //                    }.ToString());

    //                }
    //            });
    //        });
    //    }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
