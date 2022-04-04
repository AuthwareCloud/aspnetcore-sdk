namespace Authware.Blazor.Models;

public class AuthwareConfiguration
{
    /// <summary>
    ///     The app ID of your program, you still need to call InitializeApplicationAsync after injection
    /// </summary>
    public string AppId { get; set; }
}