using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Extensions;
using Turboaz.Core.Models;
using Turboaz.Core.Services;

namespace Turboaz.Presentation.Middlewares;

public class LogMiddleware : IMiddleware
{
    private readonly ICustomLogger logger;
    private readonly IContextReader contextReader;
    private readonly IDataProtector dataProtector;

    public LogMiddleware(ICustomLogger logger, IDataProtectionProvider dataProtectionProvider, IContextReader contextReader)
    {
        this.logger = logger;

        this.contextReader = contextReader;

        this.dataProtector = dataProtectionProvider.CreateProtector("TEST");
    }

    public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
    {
        if (!logger.IsLoggingEnabled())
        {
            await next.Invoke(httpContext);
        }
        else
        {
            var methodType = httpContext.Request.Method;

            var url = httpContext.Request.GetDisplayUrl();

            var userId = httpContext.Request.Cookies["Authorize"] is null ? default : Convert.ToInt16(dataProtector.Unprotect(httpContext.Request.Cookies["Authorize"]));

            var requestBody = string.Empty;

            if (httpContext.Request.Body.CanRead)
            {
                if (!httpContext.Request.Body.CanSeek)
                {
                    httpContext.Request.EnableBuffering();
                }

                requestBody = await contextReader.ReadRequest(httpContext.Request.Body);
            }

            await next.Invoke(httpContext);

            var responseBody = await contextReader.ReadResponse(httpContext.Response.Body);

            var statusCode = httpContext.Response.StatusCode;

            await this.logger.Log(new Log
            {
                UserId = userId,
                Url = url,
                MethodType = methodType,
                StatusCode = statusCode,
                RequestBody = requestBody,
                ResponseBody = responseBody
            });
        }
    }
}