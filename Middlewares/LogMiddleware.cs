using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Extensions;
using Turbo.az.Services.Base;

namespace Turbo.az.Middlewares;
public class LogMiddleware : IMiddleware
{
    private readonly ICustomLogger logger;
    private readonly IDataProtector dataProtector;
    public LogMiddleware(ICustomLogger logger, IDataProtectionProvider dataProtectionProvider)
    {
        this.logger = logger;

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

                httpContext.Request.Body.Position = 0;

                StreamReader requestReader = new(httpContext.Request.Body, Encoding.UTF8);

                requestBody = await requestReader.ReadToEndAsync();

                httpContext.Request.Body.Position = 0;
            }

            var responseBody = string.Empty;

            Stream originalBody = httpContext.Response.Body;

            using (var memStream = new MemoryStream())
            {
                httpContext.Response.Body = memStream;

                await next.Invoke(httpContext);

                memStream.Position = 0;

                StreamReader responseReader = new(httpContext.Response.Body, Encoding.UTF8);

                responseBody = await responseReader.ReadToEndAsync();

                memStream.Position = 0;

                await memStream.CopyToAsync(originalBody);
            }

            var statusCode = httpContext.Response.StatusCode;

            httpContext.Response.Body = originalBody;

            await this.logger.Log(new Models.Log
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