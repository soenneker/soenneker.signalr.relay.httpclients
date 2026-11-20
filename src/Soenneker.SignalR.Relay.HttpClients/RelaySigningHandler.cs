using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Soenneker.SignalR.Relay.HttpClients;

/// <summary>Signs management requests after Kiota expands the route parameters.</summary>
/// <param name="endpoint">Origin authorized to receive requests.</param>
/// <param name="credentials">Relay signing credentials.</param>
internal sealed class RelaySigningHandler(Uri endpoint, SigningCredentials credentials) : DelegatingHandler
{
    private readonly JsonWebTokenHandler _tokens = new();

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var uri = request.RequestUri ?? throw new InvalidOperationException("A request URI is required.");
        if (!string.Equals(uri.Scheme, endpoint.Scheme, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(uri.IdnHost, endpoint.IdnHost, StringComparison.OrdinalIgnoreCase) || uri.Port != endpoint.Port || uri.UserInfo.Length != 0)
            throw new InvalidOperationException("The request leaves the configured relay origin.");
        if (uri.AbsolutePath.StartsWith("/api/hubs/", StringComparison.OrdinalIgnoreCase))
        {
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = "azure-signalr",
                Audience = $"{endpoint.Scheme}://{endpoint.Host}{PathString.FromUriComponent(uri).ToUriComponent()}",
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = credentials
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokens.CreateToken(descriptor));
        }
        return base.SendAsync(request, cancellationToken);
    }
}
