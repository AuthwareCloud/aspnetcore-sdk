using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Authware.AspNetCore.Exceptions;
using Authware.AspNetCore.Models;

namespace Authware.AspNetCore;

public sealed class Requester
{
    /// <summary>
    ///     This is the <see cref="HttpClient" /> the <see cref="Requester" /> uses to make HTTP Requests
    /// </summary>
    private readonly IHttpClientFactory _factory;

    private HttpClient CreateClient(string? authToken = null)
    {
        var client = _factory.CreateClient("authware");
        client = new HttpClient(new HttpClientHandler
        {
            UseProxy = false,
            Proxy = null,
            ServerCertificateCustomValidationCallback = (_, certificate2, _, _) =>
                certificate2.IssuerName.Name!.Contains("CN=Cloudflare Inc ECC CA-3, O=Cloudflare, Inc., C=US") ||
                certificate2.IssuerName.Name.Contains(", O=Let's Encrypt, C=US")
        })
        {
            BaseAddress = new Uri("https://api.authware.org")
        };
        if (authToken is not null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        }
        client.DefaultRequestHeaders.TryAddWithoutValidation("X-Authware-App-Version",
            Assembly.GetEntryAssembly()?.GetName().Version.ToString());
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("Authware-DotNet",
            Assembly.GetAssembly(typeof(AuthwareApplication)).GetName().Version.ToString()));

        return client;
    }

    /// <summary>
    ///     This is for internal use,
    ///     it sets the HttpClient to be a base url for api.authware.org and adds certificate validation to prevent forgery
    /// </summary>
    public Requester(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    /// <summary>
    ///     Makes an http request using the <see cref="HttpClient" /> you passed into the constructor and automatically parses
    ///     the response to the class or struct specified
    ///     in the generic parameter to this class
    /// </summary>
    /// <param name="method">The HTTP method you want to make the request with</param>
    /// <param name="url">The URL you want to request data from</param>
    /// <param name="postData">Any data you want to post in the JSON body of the request</param>
    /// <typeparam name="T">The type you want to deserialize from JSON to</typeparam>
    /// <returns>The parsed class you specified in the generic parameter</returns>
    /// <exception cref="AuthwareException">
    ///     If the endpoint was an authware endpoint it will return this with the <see cref="ErrorResponse" />
    ///     parsed
    /// </exception>
    /// <exception cref="Exception">
    ///     Returns an exception if the API returned JSON either not able to be parsed back to the class specified
    ///     or there was an error with the request
    /// </exception>
    /// <remarks>
    ///     This class is meant to be used with the Authware.AspNetCore wrapper it is only exposed for ease of use for users of the
    ///     wrapper.
    ///     It is discouraged to use this to make requests as the exceptions it throws does specify Authware.AspNetCore issues
    /// </remarks>
    internal async Task<T> Request<T>(HttpMethod method, string url, object? postData, string? authToken = null)
    {
        using var client = CreateClient(authToken);
        using var request = new HttpRequestMessage(method, url);
        request.Headers.TryAddWithoutValidation("X-Request-DateTime",
            DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString());
        if (postData is not null)
            request.Content = new StringContent(JsonSerializer.Serialize(postData), Encoding.UTF8,
                "application/json");

        using var response = await client.SendAsync(request).ConfigureAwait(false);
        var content = await response.Content.ReadAsStringAsync();

        try
        {
            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<T>(content)!;
        }
        catch (JsonException e)
        {
            throw new Exception("There was an error when parsing the response from the authware api. \n" +
                                "The code returned from the api was a success status code. \n" +
                                "Api responses most likely changed and you need to update the the wrapper to fix this error. \n" +
                                "The response from the api was. \n" +
                                $"{content} \n", e);
        }

        try
        {
            if ((int) response.StatusCode == 429)
            {
                var retryAfter = response.Headers.RetryAfter.Delta!.Value;
                if (content.Contains("<")) throw new RateLimitException(null, retryAfter);

                throw new RateLimitException(JsonSerializer.Deserialize<ErrorResponse>(content), retryAfter);
            }

            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(content);
            if (errorResponse?.Code == BaseResponse.ResponseStatus.UpdateRequired)
                throw new UpdateRequiredException(response.Headers.GetValues("X-Authware.AspNetCore-Updater-URL").First(),
                    errorResponse);
            throw new AuthwareException(errorResponse);
        }
        catch (NullReferenceException e)
        {
            throw new Exception(
                "A non success status code was returned from the Authware.AspNetCore API. " +
                "While attempting to parse an error response from the Authware.AspNetCore API another exception occured", e);
        }
        catch (JsonException e)
        {
            throw new Exception(
                "A non success status code was returned from the Authware.AspNetCore API. " +
                "While attempting to parse an error response from the Authware.AspNetCore API another exception occured", e);
        }
    }
}