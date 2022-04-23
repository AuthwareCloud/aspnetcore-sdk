namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents a response from an executed API
/// </summary>
public class ApiResponse
{
    [JsonConstructor]
    public ApiResponse(Guid requestId, bool success, string? message = null, string? encodedResponse = null)
    {
        EncodedResponse = encodedResponse;
        RequestId = requestId;
        Message = message;
        Success = success;
    }

    /// <summary>
    ///     The message from the Authware.AspNetCore API
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; }

    /// <summary>
    ///     The request ID generated by the designated proxy server, this can be used to fetch the specific request details
    ///     back from the user
    /// </summary>
    [JsonPropertyName("request_id")]
    public Guid RequestId { get; }

    /// <summary>
    ///     The Base64 encoded response from your API
    /// </summary>
    [JsonPropertyName("response")]
    public string? EncodedResponse { get; }

    /// <summary>
    ///     If the request was successful
    /// </summary>
    [JsonPropertyName("is_success")]
    public bool Success { get; }

    /// <summary>
    ///     The plaintext response which has been decoded from Base64, this will be null if the response could not be decoded
    /// </summary>
    public string? DecodedResponse
    {
        get
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(EncodedResponse));
            }
            catch (Exception)
            {
                // ignored, if not caught and the API is configured to not respond, this will throw an exception.
                return null;
            }
        }
    }

    /// <summary>
    ///     Whether the response is going to be an actual response served by the API or a status code, due to the configuration
    ///     of your API.
    /// </summary>
    public bool CanReturnResponse
    {
        get
        {
            try
            {
                return !int.TryParse(Encoding.UTF8.GetString(Convert.FromBase64String(EncodedResponse)), out _);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public override string? ToString()
    {
        return DecodedResponse;
    }
}