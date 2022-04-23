namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents a role that the authenticated user may posses
/// </summary>
public class Role
{
    [JsonConstructor]
    public Role(Guid id, string name, Variable[]? variables = null)
    {
        Id = id;
        Name = name;
        Variables = variables;
    }

    /// <summary>
    ///     The ID of the role
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    ///     The name of the role
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    ///     The variables that the role possesses
    /// </summary>
    [JsonPropertyName("variables")]
    public Variable[]? Variables { get; }

    public override string ToString()
    {
        return $"{Name} ({Id})";
    }
}