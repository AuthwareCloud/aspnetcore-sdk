namespace Authware.AspNetCore.Models;

/// <summary>
///     Allows the configuration for Authware
/// </summary>
public class AuthwareConfiguration
{
    /// <summary>
    ///     Constructs the configuration, you can also use a simplified object initialization too
    /// </summary>
    /// <param name="appId">The ID of your application</param>
    public AuthwareConfiguration(string appId)
    {
        AppId = appId;
    }

    /// <summary>
    ///     Constructs the configuration with no parameters
    /// </summary>
    public AuthwareConfiguration()
    {
    }

    /// <summary>
    ///     The app ID of your program, you still need to call InitializeApplicationAsync after injection
    /// </summary>
    public string AppId { get; set; } = null!;
}