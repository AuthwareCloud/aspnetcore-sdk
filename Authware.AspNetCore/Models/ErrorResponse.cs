namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents an exception thrown by the Authware.AspNetCore API
/// </summary>
public class ErrorResponse : BaseResponse
{
    [JsonConstructor]
    public ErrorResponse(string? trace, ResponseStatus code, string? message = null, List<string>? errors = null) :
        base(code, message)
    {
        Trace = trace;
        Errors = errors;
    }

    /// <summary>
    ///     If the error was 500 this is the trace for the error
    /// </summary>
    [JsonPropertyName("trace")]
    public string? Trace { get; set; }

    /// <summary>
    ///     A list of errors that were thrown by the Authware.AspNetCore API, these are commonly data validation errors
    /// </summary>
    [JsonPropertyName("errors")]
    public List<string>? Errors { get; set; }

    /// <summary>
    ///     Converts the ErrorResponse to a friendly format for display
    /// </summary>
    /// <returns>The formatted error response</returns>
    public override string ToString()
    {
        var errors = Errors?.Aggregate(string.Empty, (current, item) => current + $"{item}, ")
            .TrimEnd(' ')
            .TrimEnd(',');

        return $"{base.ToString()} {(errors is null ? string.Empty : $"({errors})")}";
    }
}