using System;
using System.Text.Json.Serialization;


namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents a role that the authenticated user may posses
/// </summary>
public class Role
{
    /// <summary>
    ///     The ID of the role
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     The name of the role
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    ///     The variables that the role possesses
    /// </summary>
    [JsonPropertyName("variables")]
    public Variable[]? Variables { get; set; }

    public override string ToString()
    {
        return $"{Name} ({Id})";
    }
}