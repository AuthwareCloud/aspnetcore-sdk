namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents the data returned from an application data request
/// </summary>
public class Application
{
    [JsonConstructor]
    public Application(string name, Guid id, Version version, DateTime created, bool checkIdentifier, int userCount,
        int requestCount, Api[]? apis = null)
    {
        Name = name;
        Id = id;
        Version = version;
        Created = created;
        CheckIdentifier = checkIdentifier;
        Apis = apis;
        UserCount = userCount;
        RequestCount = requestCount;
    }

    /// <summary>
    ///     The friendly name of your application
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    ///     The ID of your application
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    ///     The set version of your application
    /// </summary>
    [JsonPropertyName("version")]
    public Version Version { get; }

    /// <summary>
    ///     The date your application was created
    /// </summary>
    [JsonPropertyName("date_created")]
    public DateTime Created { get; }

    /// <summary>
    ///     Tells you if your application has hwid locking enabled
    /// </summary>
    [JsonPropertyName("is_hwid_checking_enabled")]
    public bool CheckIdentifier { get; }

    /// <summary>
    ///     A list of APIs that is implemented in your application
    /// </summary>
    [JsonPropertyName("apis")]
    public Api[]? Apis { get; }

    [JsonPropertyName("user_count")] public int UserCount { get; }
    [JsonPropertyName("request_count")] public int RequestCount { get; }

    public override string ToString()
    {
        return $"{Name} (v{Version})";
    }
}