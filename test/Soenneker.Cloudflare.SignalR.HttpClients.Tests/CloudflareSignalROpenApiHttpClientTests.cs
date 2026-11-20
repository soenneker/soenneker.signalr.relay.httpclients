using Soenneker.Cloudflare.SignalR.HttpClients.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Cloudflare.SignalR.HttpClients.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class CloudflareSignalROpenApiHttpClientTests : HostedUnitTest
{
    private readonly ICloudflareSignalROpenApiHttpClient _httpclient;

    public CloudflareSignalROpenApiHttpClientTests(Host host) : base(host)
    {
        _httpclient = Resolve<ICloudflareSignalROpenApiHttpClient>(true);
    }

    [Test]
    public async System.Threading.Tasks.Task RejectsRequestsOutsideTheConfiguredOrigin()
    {
        var client = await _httpclient.Get();
        await Assert.That(async () => await client.GetAsync("https://other.invalid/api/hubs/test/:send"))
            .Throws<System.InvalidOperationException>();
    }
}
