using Newtonsoft.Json;

namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents the authentication token returned by a successful authentication request
/// </summary>
public class AuthResponse
{
    /// <summary>
    ///     The Authware.AspNetCore authentication token returned from the API, this can be used to authorize Authware.AspNetCore API requests for
    ///     your application
    /// </summary>
    [JsonProperty("auth_token")]
    public string AuthToken { get; set; }

    public override string ToString()
    {
        return AuthToken;
    }
}