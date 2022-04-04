using System;
using System.Text.Json.Serialization;

namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents the data returned from an application data request
/// </summary>
public class Application
{
    /// <summary>
    ///     The friendly name of your application
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    ///     The ID of your application
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     The set version of your application
    /// </summary>
    [JsonPropertyName("version")]
    public Version Version { get; set; }

    /// <summary>
    ///     The date your application was created
    /// </summary>
    [JsonPropertyName("date_created")]
    public DateTime Created { get; set; }

    /// <summary>
    ///     Tells you if your application has hwid locking enabled
    /// </summary>
    [JsonPropertyName("is_hwid_checking_enabled")]
    public bool CheckIdentifier { get; set; }

    /// <summary>
    ///     A list of APIs that is implemented in your application
    /// </summary>
    [JsonPropertyName("apis")]
    public Api[]? Apis { get; set; }

    public override string ToString()
    {
        return $"{Name} (v{Version})";
    }
}