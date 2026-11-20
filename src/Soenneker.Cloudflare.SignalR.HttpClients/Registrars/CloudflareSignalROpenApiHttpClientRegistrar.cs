using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Cloudflare.SignalR.HttpClients.Abstract;

namespace Soenneker.Cloudflare.SignalR.HttpClients.Registrars;

/// <summary>
/// Registers the OpenAPI HttpClient wrapper for dependency injection.
/// </summary>
public static class CloudflareSignalROpenApiHttpClientRegistrar
{
    /// <summary>
    /// Adds <see cref="CloudflareSignalROpenApiHttpClient"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddCloudflareSignalROpenApiHttpClientAsSingleton(this IServiceCollection services)
    {
        services.TryAddSingleton<ICloudflareSignalROpenApiHttpClient, CloudflareSignalROpenApiHttpClient>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="CloudflareSignalROpenApiHttpClient"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddCloudflareSignalROpenApiHttpClientAsScoped(this IServiceCollection services)
    {
        services.TryAddScoped<ICloudflareSignalROpenApiHttpClient, CloudflareSignalROpenApiHttpClient>();

        return services;
    }
}
