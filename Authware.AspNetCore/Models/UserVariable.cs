namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents a variable possessed by a user
/// </summary>
public class UserVariable : Variable
{
    [JsonConstructor]
    public UserVariable(string key, string value, bool canUserEdit) : base(key, value)
    {
        CanUserEdit = canUserEdit;
    }

    /// <summary>
    ///     If true, the signed-in user can edit and delete this variable
    /// </summary>
    [JsonPropertyName("can_user_edit")]
    public bool CanUserEdit { get; }
}