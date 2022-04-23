namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents the authentication token returned by a successful authentication request
/// </summary>
public class AuthResponse
{
    [JsonConstructor]
    public AuthResponse(string authToken)
    {
        AuthToken = authToken;
    }

    /// <summary>
    ///     The Authware authentication token returned from the API, this can be used to authorize Authware API requests for
    ///     your application
    /// </summary>
    [JsonPropertyName("auth_token")]
    public string AuthToken { get; }

    public override string ToString()
    {
        return AuthToken;
    }
}