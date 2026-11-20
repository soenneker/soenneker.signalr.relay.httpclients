using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Soenneker.SignalR.Relay.HttpClients.Abstract;

namespace Soenneker.SignalR.Relay.HttpClients;

/// <summary>Owns a pooled HTTP transport for one configured relay origin.</summary>
public sealed class SignalRRelayOpenApiHttpClient : ISignalRRelayOpenApiHttpClient
{
    private readonly HttpClient _client;

    /// <summary>Reads the relay origin and signing key for this client.</summary>
    /// <param name="configuration">Configuration containing Relay:Endpoint and Relay:AccessKey.</param>
    public SignalRRelayOpenApiHttpClient(IConfiguration configuration)
    {
        var endpoint = new Uri(configuration["Relay:Endpoint"] ?? throw new InvalidOperationException("Relay:Endpoint is required."));
        if ((endpoint.Scheme != "https" && !(endpoint.Scheme == "http" && endpoint.IsLoopback)) ||
            endpoint.AbsolutePath != "/" || endpoint.UserInfo.Length != 0 || endpoint.Query.Length != 0 || endpoint.Fragment.Length != 0)
            throw new InvalidOperationException("Relay:Endpoint must be an HTTPS origin or a loopback HTTP origin.");
        var key = Encoding.UTF8.GetBytes(configuration["Relay:AccessKey"] ?? "");
        if (key.Length < 32) throw new InvalidOperationException("Relay:AccessKey must contain at least 32 bytes.");
        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
        _client = new HttpClient(new RelaySigningHandler(endpoint, credentials)
        {
            InnerHandler = new SocketsHttpHandler { AllowAutoRedirect = false, PooledConnectionLifetime = TimeSpan.FromMinutes(5) }
        }) { BaseAddress = endpoint };
    }

    public ValueTask<HttpClient> Get(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return ValueTask.FromResult(_client);
    }

    public void Dispose() => _client.Dispose();

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }
}
