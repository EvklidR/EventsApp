using AuthorisationService.Application.Exceptions;
using Azure;
using Newtonsoft.Json;
using System.Net;

namespace AuthorisationService.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string result;

            switch (exception)
            {
                case AlreadyExistsException alreadyExistsEx:
                    statusCode = HttpStatusCode.Conflict;
                    result = alreadyExistsEx.Message;
                    break;
                case BadRequestException badRequestEx:
                    statusCode = HttpStatusCode.BadRequest;
                    result = badRequestEx.Message;
                    break;
                case BadAuthorisationException notFoundEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    result = notFoundEx.Message;
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    result = "An unexpected error occurred.";
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
