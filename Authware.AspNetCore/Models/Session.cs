namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents an active session that the user possesses
/// </summary>
public class Session
{
    [JsonConstructor]
    public Session(Guid id, DateTime dateCreated)
    {
        Id = id;
        DateCreated = dateCreated;
    }

    /// <summary>
    ///     The ID of the session
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    ///     The date the session was created
    /// </summary>

    [JsonPropertyName("date_created")]
    public DateTime DateCreated { get; }

    public override string ToString()
    {
        return Id.ToString();
    }
}