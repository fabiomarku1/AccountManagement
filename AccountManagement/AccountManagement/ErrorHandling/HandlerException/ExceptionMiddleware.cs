using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using System.Net.Http;

namespace AccountManagement.ErrorHandling.HandlerException
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            //catch ( ex)
            //{
            //    await HandleExceptionAsync(httpContext, ex);
            //}
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {

                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.NotFound;


            await context.Response.WriteAsync(new ErrorModel()
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message
            }.ToString());
        }

    }
}