using System;
using System.Text.Json.Serialization;


namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents an active session that the user possesses
/// </summary>
public class Session
{
    /// <summary>
    ///     The ID of the session
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     The date the session was created
    /// </summary>

    [JsonPropertyName("date_created")]
    public DateTime DateCreated { get; set; }

    public override string ToString()
    {
        return Id.ToString();
    }
}