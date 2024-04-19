using System.Text;
using Turboaz.Core.Services;

namespace Turboaz.Infrastructure.Services;

public class HttpContextReader : IContextReader
{
    public async Task<string?> ReadRequest(Stream request)
    {
        request.Position = 0;

        StreamReader requestReader = new(request, Encoding.UTF8);

        var requestBody = await requestReader.ReadToEndAsync();

        request.Position = 0;

        return requestBody;
    }

    public async Task<string?> ReadResponse(Stream response)
    {
        Stream originalBody = response;

        var responseBody = string.Empty;

        using (var memStream = new MemoryStream())
        {
            response = memStream;

            memStream.Position = 0;

            StreamReader responseReader = new(response, Encoding.UTF8);

            responseBody = await responseReader.ReadToEndAsync();

            memStream.Position = 0;

            await memStream.CopyToAsync(originalBody);
        }

        response = originalBody;

        return responseBody;
    }
}