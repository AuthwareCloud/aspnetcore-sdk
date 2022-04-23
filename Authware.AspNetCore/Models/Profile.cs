namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents the currently authenticated users profile
/// </summary>
public class Profile
{
    [JsonConstructor]
    public Profile(string username, Guid id, string email, DateTime dateCreated, DateTime planExpire,
        Session[]? sessions = null, Role? role = null, ApiRequest[]? apiRequests = null,
        UserVariable[]? userVariables = null)
    {
        Username = username;
        Id = id;
        Email = email;
        DateCreated = dateCreated;
        PlanExpire = planExpire;
        Sessions = sessions;
        Role = role;
        ApiRequests = apiRequests;
        UserVariables = userVariables;
    }

    /// <summary>
    ///     The username of the user
    /// </summary>
    [JsonPropertyName("username")]
    public string Username { get; }

    /// <summary>
    ///     The ID of the user
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    ///     The email of the user
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; }

    /// <summary>
    ///     The date the user was created
    /// </summary>

    [JsonPropertyName("date_created")]
    public DateTime DateCreated { get; }

    /// <summary>
    ///     The date the users access will expire
    /// </summary>
    [JsonPropertyName("expiration")]
    public DateTime PlanExpire { get; }

    /// <summary>
    ///     A list of active sessions that the may user possesses
    /// </summary>

    [JsonPropertyName("sessions")]
    public Session[]? Sessions { get; }

    /// <summary>
    ///     A list of roles the user possesses
    /// </summary>

    [JsonPropertyName("role")]
    public Role? Role { get; }

    /// <summary>
    ///     A list of previous API requests that this user has performed
    /// </summary>
    [JsonPropertyName("requests")]
    public ApiRequest[]? ApiRequests { get; }

    /// <summary>
    ///     A list of variables that the user possesses
    /// </summary>
    [JsonPropertyName("user_variables")]
    public UserVariable[]? UserVariables { get; }

    public override string ToString()
    {
        return $"{Username} ({Id})";
    }
}