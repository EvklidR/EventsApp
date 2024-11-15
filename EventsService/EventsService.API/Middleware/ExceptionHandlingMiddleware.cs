using EventsService.Application.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace EventsService.API.Middleware
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
                case NotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    result = notFoundEx.Message;
                    break;
                case BusinessLogicException businessLogicEx:
                    statusCode = HttpStatusCode.Conflict;
                    result = businessLogicEx.Message;
                    break;
                case BadAuthorisationException badAuthEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    result = badAuthEx.Message;
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
