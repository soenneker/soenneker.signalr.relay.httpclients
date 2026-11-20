using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Soenneker.Cloudflare.SignalR.HttpClients.Abstract;

/// <summary>
/// Provides the HTTP transport for a SignalR relay.
/// </summary>
public interface ICloudflareSignalROpenApiHttpClient: IDisposable, IAsyncDisposable
{
    /// <summary>Returns the transport shared by this service instance.</summary>
    /// <param name="cancellationToken">Cancels retrieval.</param>
    /// <returns>The configured HTTP client.</returns>
    ValueTask<HttpClient> Get(CancellationToken cancellationToken = default);
}
