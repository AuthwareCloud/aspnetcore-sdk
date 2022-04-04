using System;
using System.Text.Json.Serialization;


namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents the currently authenticated users profile
/// </summary>
public class Profile
{
    /// <summary>
    ///     The username of the user
    /// </summary>
    [JsonPropertyName("username")]
    public string Username { get; set; }

    /// <summary>
    ///     The ID of the user
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     The email of the user
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }

    /// <summary>
    ///     The date the user was created
    /// </summary>

    [JsonPropertyName("date_created")]
    public DateTime DateCreated { get; set; }

    /// <summary>
    ///     The date the users access will expire
    /// </summary>
    [JsonPropertyName("plan_expire")]
    public DateTime PlanExpire { get; set; }

    /// <summary>
    ///     A list of active sessions that the may user possesses
    /// </summary>

    [JsonPropertyName("sessions")]
    public Session[]? Sessions { get; set; }

    /// <summary>
    ///     A list of roles the user possesses
    /// </summary>

    [JsonPropertyName("role")]
    public Role? Role { get; set; }

    /// <summary>
    ///     A list of previous API requests that this user has performed
    /// </summary>
    [JsonPropertyName("requests")]
    public ApiRequest[]? ApiRequests { get; set; }

    /// <summary>
    ///     A list of variables that the user possesses
    /// </summary>
    [JsonPropertyName("user_variables")]
    public UserVariable[]? UserVariables { get; set; }

    public override string ToString()
    {
        return $"{Username} ({Id})";
    }
}