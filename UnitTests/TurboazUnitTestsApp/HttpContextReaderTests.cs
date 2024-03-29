namespace TurboazUnitTestsApp;

using Turboaz.Core.Services;
using Turboaz.Infrastructure.Services;

public class HttpContextReaderTests
{
    private readonly IContextReader contextReader;
    public HttpContextReaderTests() => this.contextReader = new HttpContextReader();

    [Fact]
    public void ReadRequest_PassNullRequestStream_ThrowsArgumentNullException()
    {
        Stream requestStream = null;

        Assert.ThrowsAsync<ArgumentNullException>(async () => await contextReader.ReadRequest(requestStream));
    }

    [Fact]
    public void ReadResponse_PassNullResponseStream_ThrowsArgumentNullException()
    {
        Stream responseStream = null;

        Assert.ThrowsAsync<ArgumentNullException>(async () => await contextReader.ReadResponse(responseStream));
    }
}