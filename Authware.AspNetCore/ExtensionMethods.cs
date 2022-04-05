using System;
using System.Collections.Generic;
using System.Security.Claims;
using Authware.AspNetCore.Models;
using Authware.AspNetCore.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Authware.AspNetCore;

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

        services.AddSingleton<Requester>();

        return services.AddSingleton(x => new AuthwareApplication(config.AppId, x.GetRequiredService<Requester>()));
    }

    /// <summary>
    /// Converts a <see cref="Profile"/> to a <see cref="AuthwareClaimsPrincipal"/> for usage with ASP.NET Core's default authorization scheme
    /// </summary>
    /// <param name="profile">The profile to convert</param>
    /// <returns>The claims principal of the profile</returns>
    public static AuthwareClaimsPrincipal ConvertToClaimsPrincipal(this Profile profile)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, profile.Email),
            new(ClaimTypes.NameIdentifier, profile.Id.ToString()),
            new(ClaimTypes.Name, profile.Username),
            new(ClaimTypes.AuthenticationMethod, "Authware"),
            new(ClaimTypes.Webpage, "https://authware.org")
        };

        if (profile.Role is not null)
        {
            claims.Add(new Claim(ClaimTypes.Role, profile.Role.Name));
        }

        return new AuthwareClaimsPrincipal(claims, profile);
    }
}