namespace Turboaz.Core.Services;

public interface IContextReader
{
    Task<string?> ReadRequest(Stream request);
    Task<string?> ReadResponse(Stream response);
}