using Microsoft.AspNetCore.Http;
using TinyUrl.AuthenticationService.Infrastructure.Common;
using TinyUrl.AuthenticationService.Infrastructure.Exceptions;
using System.Text.Json;
using System.Net;


namespace TinyUrl.AuthenticationService.Infrastructure.Middlewares
{
    public class UnauthorizedHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    throw new UnauthorizedException(ErrorMessages.TokenExpiredErrorMessage);
                }
            }
            catch (UnauthorizedException ex)
            {
                var error = ObjectResultCreator.To401UnauthorizedResult(ex.Message, ErrorTitles.UnauthorizedErrorTitle);

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                var jsonResponse = JsonSerializer.Serialize(error.Value);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
