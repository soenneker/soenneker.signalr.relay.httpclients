using Soenneker.SignalR.Relay.HttpClients.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.SignalR.Relay.HttpClients.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public sealed class SignalRRelayOpenApiHttpClientTests : HostedUnitTest
{
    private readonly ISignalRRelayOpenApiHttpClient _httpclient;

    public SignalRRelayOpenApiHttpClientTests(Host host) : base(host)
    {
        _httpclient = Resolve<ISignalRRelayOpenApiHttpClient>(true);
    }

    [Test]
    public async System.Threading.Tasks.Task RejectsRequestsOutsideTheConfiguredOrigin()
    {
        var client = await _httpclient.Get();
        await Assert.That(async () => await client.GetAsync("https://other.invalid/api/hubs/test/:send"))
            .Throws<System.InvalidOperationException>();
    }
}
