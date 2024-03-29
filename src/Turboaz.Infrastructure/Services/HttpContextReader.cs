using System.Text;
using Turboaz.Core.Services;

namespace Turboaz.Infrastructure.Services;

public class HttpContextReader : IContextReader
{
    public async Task<string?> ReadRequest(Stream request)
    {
        if (request is null)
        {
            throw new ArgumentNullException("Request stream cannot be null");
        }

        request.Position = 0;

        StreamReader requestReader = new(request, Encoding.UTF8);

        var requestBody = await requestReader.ReadToEndAsync();

        request.Position = 0;

        return requestBody;
    }

    public async Task<string?> ReadResponse(Stream response)
    {
        if (response is null)
        {
            throw new ArgumentNullException("Response stream cannot be null");
        }

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