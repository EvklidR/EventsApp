using AuthorisationService.Application.Exceptions;
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
            object result;

            switch (exception)
            {
                case AlreadyExistsException alreadyExistsEx:
                    statusCode = HttpStatusCode.Conflict;
                    result = new { message = alreadyExistsEx.Message };
                    break;

                case BadRequestException badRequestEx:
                    statusCode = HttpStatusCode.BadRequest;
                    result = new { message = new { errors = badRequestEx.Errors } };
                    break;

                case UnauthorizedException badAuthEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    result = new { message = badAuthEx.Message };
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    result = new { message = exception.Message };
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
