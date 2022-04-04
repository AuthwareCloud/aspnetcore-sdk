using System;
using Authware.Blazor.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Authware.Blazor;

public static class ExtensionMethods
{
    /// <summary>
    ///     Adds Authware to the service provider, including the required services such as the <see cref="Requester" />
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration to inject</param>
    /// <returns>The new service collection with Authware added</returns>
    /// <exception cref="ArgumentNullException">Thrown when the configuration is null, it must be set</exception>
    public static IServiceCollection AddAuthware(this IServiceCollection services,
        Action<AuthwareConfiguration> configuration)
    {
        _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

        var config = new AuthwareConfiguration();
        configuration(config);

        services.AddScoped<Requester>();

        return services.AddScoped(x => new AuthwareApplication(config.AppId, x.GetRequiredService<Requester>()));
    }
}