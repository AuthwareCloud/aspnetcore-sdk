namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents an API that has been added to your application, this contains the name of your API and the ID, this can
///     be useful for listing your APIs in a list for the user to decide what to execute.
/// </summary>
public class Api
{
    [JsonConstructor]
    public Api(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    /// <summary>
    ///     The ID of your API
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     The friendly name of your API
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}