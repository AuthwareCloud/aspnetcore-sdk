namespace Authware.AspNetCore.Models;

/// <summary>
///     A wrapper around a instanced Authware.AspNetCore request
/// </summary>
/// <typeparam name="TResponse">The type of response that is being wrapped around</typeparam>
public class StaticResponse<TResponse> : ErrorResponse
{
    [JsonConstructor]
    public StaticResponse(TResponse response, ResponseStatus code, string? trace = null, string? message = null,
        List<string>? errors = null) : base(trace, code, message, errors)
    {
        Response = response;
    }

    /// <summary>
    ///     The response received by the Authware.AspNetCore API, this is the data that is returned on a successful response
    /// </summary>
    public TResponse Response { get; }
}