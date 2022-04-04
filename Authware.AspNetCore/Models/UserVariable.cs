﻿using System.Text.Json.Serialization;


namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents a variable possessed by a user
/// </summary>
public class UserVariable : Variable
{
    /// <summary>
    ///     If true, the signed-in user can edit and delete this variable
    /// </summary>
    [JsonPropertyName("can_user_edit")]
    public bool CanUserEdit { get; set; }
}