namespace Authware.Blazor.Models;

/// <summary>
///     A wrapper around a instanced Authware.Blazor request
/// </summary>
/// <typeparam name="TResponse">The type of response that is being wrapped around</typeparam>
public class StaticResponse<TResponse> : ErrorResponse
{
    /// <summary>
    ///     The response received by the Authware.Blazor API, this is the data that is returned on a successful response
    /// </summary>
    public TResponse Response { get; set; }
}