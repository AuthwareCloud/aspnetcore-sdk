using System.Text.Json.Serialization;


namespace Authware.AspNetCore.Models;

/// <summary>
///     Represents a <see cref="BaseResponse" /> but includes new or modified data for your application to update with
/// </summary>
/// <typeparam name="TData">The new or updated data</typeparam>
public class UpdatedDataResponse<TData> : BaseResponse
{
    /// <summary>
    ///     The new or modified data
    /// </summary>
    [JsonPropertyName("new_data")]
    public TData NewData { get; set; }
}