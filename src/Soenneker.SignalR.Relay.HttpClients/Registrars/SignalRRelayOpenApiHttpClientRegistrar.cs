using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.SignalR.Relay.HttpClients.Abstract;

namespace Soenneker.SignalR.Relay.HttpClients.Registrars;

/// <summary>
/// Registers the OpenAPI HttpClient wrapper for dependency injection.
/// </summary>
public static class SignalRRelayOpenApiHttpClientRegistrar
{
    /// <summary>
    /// Adds <see cref="SignalRRelayOpenApiHttpClient"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddSignalRRelayOpenApiHttpClientAsSingleton(this IServiceCollection services)
    {
        services.TryAddSingleton<ISignalRRelayOpenApiHttpClient, SignalRRelayOpenApiHttpClient>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="SignalRRelayOpenApiHttpClient"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddSignalRRelayOpenApiHttpClientAsScoped(this IServiceCollection services)
    {
        services.TryAddScoped<ISignalRRelayOpenApiHttpClient, SignalRRelayOpenApiHttpClient>();

        return services;
    }
}
