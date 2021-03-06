namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents a variable from either an application or a role
/// </summary>
public class Variable
{
    [JsonConstructor]
    public Variable(string key, string value)
    {
        Key = key;
        Value = value;
    }

    /// <summary>
    ///     The key of the variable
    /// </summary>
    [JsonPropertyName("key")]
    public string Key { get; }

    /// <summary>
    ///     The value of the variable
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; }

    /// <summary>
    ///     This is mostly used for deconstructing types to a tuple
    /// </summary>
    /// <param name="key">The key of the variable</param>
    /// <param name="value">The value of the variable</param>
    public void Deconstruct(out string key, out string value)
    {
        key = Key;
        value = Value;
    }

    /// <summary>
    ///     Gives you the variable in Key: Value format
    /// </summary>
    /// <returns>The variable formatted into {Key}: {Value}</returns>
    public override string ToString()
    {
        return $"{Key}: {Value}";
    }
}