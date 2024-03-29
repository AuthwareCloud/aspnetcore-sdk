namespace Authware.AspNetCore;

/// <summary>
///     The main Authware application class, this contains all of the functions that can be used to interact with
///     the Authware API
/// </summary>
public sealed class AuthwareApplication
{
    /// <summary>
    ///     This is used to facilitate HTTP requests for this class
    /// </summary>
    private readonly Requester _requester;

    /// <summary>
    ///     The ID of the current application
    /// </summary>
    private string? _applicationId;

    /// <summary>
    ///     Constructs the application class instance with default values
    /// </summary>
    public AuthwareApplication(string applicationId, Requester requester)
    {
        _applicationId = applicationId;
        _requester = requester;
    }

    /// <summary>
    ///     Stores the information responded by <see cref="InitializeApplicationAsync" /> for easy access
    /// </summary>
    public Application? ApplicationInformation { get; private set; }

    /// <summary>
    ///     Initializes and checks the ID passed in against the Authware API to make sure the application is properly
    ///     setup and
    ///     enabled
    /// </summary>
    /// <returns>The application name, version and creation date represented by a <see cref="Application" /></returns>
    /// <exception cref="ArgumentNullException">Thrown if the application ID is null</exception>
    /// <exception cref="ArgumentException">Thrown if the application ID is not a valid GUID</exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if the application does not exist under the provided ID
    /// </exception>
    public async Task<Application> InitializeApplicationAsync()
    {
        if (ApplicationInformation is not null) return ApplicationInformation;

        var _ = _applicationId ??
                throw new ArgumentNullException(_applicationId, $"{nameof(_applicationId)} can not be null");
        if (!Guid.TryParse(_applicationId, out var _))
            throw new ArgumentException($"{_applicationId} is invalid");

        var applicationResponse = await _requester
            .Request<Application>(HttpMethod.Post, "/app", new { app_id = _applicationId })
            .ConfigureAwait(false);

        ApplicationInformation = applicationResponse;

        return ApplicationInformation;
    }

    /// <summary>
    ///     Creates a new user variable with the specified key and value
    /// </summary>
    /// <param name="authToken">The user's authentication token</param>
    /// <param name="key">
    ///     The key of the variable to create
    /// </param>
    /// <param name="value">
    ///     The value of the variable to create
    /// </param>
    /// <param name="canEdit">
    ///     Should the users be able to edit this variable (This can be used to make readonly variables
    /// </param>
    /// <param name="isApiKey">Whether the provided auth token is an API key or not</param>
    /// <returns>A <see cref="UpdatedDataResponse{T}" /> which contains the newly created variable</returns>
    /// <exception cref="Exception">
    ///     This gets thrown if the application id is null which would be if
    ///     <see cref="InitializeApplicationAsync" /> hasn't been called
    /// </exception>
    /// <exception cref="ArgumentNullException">If any of the parameters are null this is what gets thrown</exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if the application is disabled or you attempted to create a variable when the application has creating user
    ///     variables disabled
    /// </exception>
    public async Task<UpdatedDataResponse<UserVariable>> CreateUserVariableAsync(string authToken, string key,
        string value, bool canEdit = true, bool isApiKey = false)
    {
        _ = _applicationId ?? throw new Exception($"{nameof(_applicationId)} can not be null");
        _ = key ?? throw new ArgumentNullException(key, $"{nameof(key)} can not be null");
        _ = value ?? throw new ArgumentNullException(value, $"{nameof(value)} can not be null");

        var response = await _requester
            .Request<UpdatedDataResponse<UserVariable>>(HttpMethod.Post, "/user/variables",
                new { key, value, can_user_edit = canEdit }, authToken, isApiKey)
            .ConfigureAwait(false);

        return response;
    }

    /// <summary>
    ///     Updates a user variable by the variables key
    /// </summary>
    /// <param name="authToken">The user's authentication token</param>
    /// <param name="key">
    ///     The key of the variable to update
    /// </param>
    /// <param name="newValue">
    ///     The new value of the variable
    /// </param>
    /// <param name="isApiKey">Whether the provided auth token is an API key or not</param>
    /// <returns>A <see cref="UpdatedDataResponse{T}" /> which contains the newly created variable</returns>
    /// <exception cref="Exception">
    ///     This gets thrown if the application id is null which would be if
    ///     <see cref="InitializeApplicationAsync" /> hasn't been called
    /// </exception>
    /// <exception cref="ArgumentNullException">If any of the parameters are null this is what gets thrown</exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if the application is disabled or you attempted to modify a variable when you do not have permission to
    /// </exception>
    public async Task<UpdatedDataResponse<UserVariable>> UpdateUserVariableAsync(string authToken, string key,
        string newValue, bool isApiKey = false)
    {
        _ = _applicationId ?? throw new Exception($"{nameof(_applicationId)} can not be null");
        _ = key ?? throw new ArgumentNullException(key, $"{nameof(key)} can not be null");
        _ = newValue ?? throw new ArgumentNullException(newValue, $"{nameof(newValue)} can not be null");

        var response = await _requester
            .Request<UpdatedDataResponse<UserVariable>>(HttpMethod.Put, "/user/variables",
                new { key, value = newValue }, authToken, isApiKey)
            .ConfigureAwait(false);

        return response;
    }

    /// <summary>
    ///     Deletes a user variable by the variables key
    /// </summary>
    /// <param name="authToken">The user's authentication token</param>
    /// <param name="key">
    ///     The key of the variable to delete
    /// </param>
    /// <param name="isApiKey">Whether the provided auth token is an API key or not</param>
    /// <returns>A <see cref="BaseResponse" /> that represents whether the response succeeded or not</returns>
    /// <exception cref="Exception">
    ///     This gets thrown if the application id is null which would be if
    ///     <see cref="InitializeApplicationAsync" /> hasn't been called
    /// </exception>
    /// <exception cref="ArgumentNullException">If any of the parameters are null this is what gets thrown</exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if the application is disabled or you attempted to modify a variable when you do not have permission to
    /// </exception>
    public async Task<BaseResponse> DeleteUserVariableAsync(string authToken, string key, bool isApiKey = false)
    {
        _ = _applicationId ?? throw new Exception($"{nameof(_applicationId)} can not be null");
        _ = key ?? throw new ArgumentNullException(key, $"{nameof(key)} can not be null");

        var response = await _requester
            .Request<BaseResponse>(HttpMethod.Delete, "/user/variables",
                new { key }, authToken, isApiKey)
            .ConfigureAwait(false);

        return response;
    }

    /// <summary>
    ///     Gets all application variables that the user has permission to get
    /// </summary>
    /// <param name="authToken">The user's authentication token</param>
    /// <param name="isApiKey">Whether the provided auth token is an API key or not</param>
    /// <returns>An array of keys and values represented by a <see cref="Variable" /></returns>
    /// <exception cref="Exception">
    ///     This gets thrown if the application id is null which would be if
    ///     <see cref="InitializeApplicationAsync" /> hasn't been called
    /// </exception>
    /// <exception cref="ArgumentNullException">If any of the parameters are null this is what gets thrown</exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if the application is disabled or you attempted to fetch authenticated variables whilst not being
    ///     authenticated
    /// </exception>
    public async Task<Variable[]> GrabApplicationVariablesAsync(string? authToken = null, bool isApiKey = false)
    {
        var _ = _applicationId ?? throw new Exception($"{nameof(_applicationId)} can not be null");
        if (authToken is null)
        {
            var authenticatedVariables = await _requester
                .Request<Variable[]>(HttpMethod.Get, "/app/variables", null)
                .ConfigureAwait(false);
            return authenticatedVariables;
        }

        var variables = await _requester
            .Request<Variable[]>(HttpMethod.Post, "/app/variables", new { app_id = _applicationId }, authToken,
                isApiKey)
            .ConfigureAwait(false);
        return variables;
    }

    /// <summary>
    ///     Allows a user to create an account on your application, provided if the username, password, email and license are
    ///     valid.
    /// </summary>
    /// <param name="username">The username the user will login with</param>
    /// <param name="password">The password the user should need to login with</param>
    /// <param name="email">The email the user will have assigned getting alerts and other things at</param>
    /// <param name="token">The license/token you want to register the user with</param>
    /// <returns>The base response containing the code and status message</returns>
    /// <exception cref="Exception">
    ///     This gets thrown if the application ID is null which would be if
    ///     <see cref="InitializeApplicationAsync" /> hasn't been called
    /// </exception>
    /// <exception cref="ArgumentNullException">If any of the parameters are null this is what gets thrown</exception>
    /// <exception cref="ArgumentException">
    ///     This gets thrown if the license you want to use to register the user with is
    ///     invalid
    /// </exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if the data provided is not acceptable by the Authware API, the license
    ///     was not valid or the application is disabled
    /// </exception>
    public async Task<BaseResponse> RegisterAsync(string username, string password, string email, string token)
    {
        _ = _applicationId ?? throw new Exception($"{nameof(_applicationId)} can not be null");
        _ = username ?? throw new ArgumentNullException(username, $"{nameof(username)} can not be null");
        _ = password ?? throw new ArgumentNullException(password, $"{nameof(password)} can not be null");
        _ = email ?? throw new ArgumentNullException(email, $"{nameof(email)} can not be null");
        _ = token ?? throw new ArgumentNullException(token, $"{nameof(token)} can not be null");
        if (!Guid.TryParse(token, out var _))
            throw new ArgumentException($"{nameof(token)} is invalid");

        var response = await _requester
            .Request<BaseResponse>(HttpMethod.Post, "/user/register",
                new
                {
                    app_id = _applicationId, username, password,
                    email_address = email, token
                })
            .ConfigureAwait(false);
        return response;
    }

    // I don't like how this method looks split it up maybe? possibly make a separate method for authenticating with token to get the profile? 
    /// <summary>
    ///     Authenticates a user against the Authware API with the provided username and password and caches the
    ///     Authware
    ///     authentication token if successful
    ///     If the user has logged in before it will check the cached Authware authentication token and if the token is
    ///     invalid
    ///     it will authenticate with the username and password
    /// </summary>
    /// <param name="username">The username you want to authenticate with</param>
    /// <param name="password">The password you want to authenticate with</param>
    /// <returns>The authenticated users profile, represented as <see cref="Profile" /></returns>
    /// <exception cref="Exception">
    ///     This gets thrown if the application ID is null which would be if
    ///     <see cref="InitializeApplicationAsync" /> hasn't been called
    /// </exception>
    /// <exception cref="ArgumentNullException">If the username or password is null this exception is thrown</exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if the data provided is not acceptable by the Authware API, the hardware ID did not match (if
    ///     enabled), the
    ///     application version is out-of-date (if enabled) or the username and password are invalid
    /// </exception>
    public async Task<(AuthResponse, Profile)> LoginAsync(string username, string password)
    {
        _ = _applicationId ?? throw new Exception($"{nameof(_applicationId)} can not be null");
        _ = username ?? throw new ArgumentNullException(username, $"{nameof(username)} can not be null");
        _ = password ?? throw new ArgumentNullException(password, $"{nameof(password)} can not be null");

        var authResponse = await _requester
            .Request<AuthResponse>(HttpMethod.Post, "/user/auth",
                new { app_id = _applicationId, username, password })
            .ConfigureAwait(false);
        var profileResponse =
            await _requester.Request<Profile>(HttpMethod.Get, "user/profile", null, authResponse.AuthToken)
                .ConfigureAwait(false);

        return (authResponse, profileResponse);
    }

    /// <summary>
    ///     Redeems a registration token to a user, this is for when a user expires and purchases a new token
    /// </summary>
    /// <param name="username">The username you want to redeem the token to</param>
    /// <param name="token">The token you want to redeem</param>
    /// <returns>A base response containing details about the redemption</returns>
    /// <exception cref="Exception">
    ///     This gets thrown if the application ID is null which would be if
    ///     <see cref="InitializeApplicationAsync" /> hasn't been called
    /// </exception>
    /// <exception cref="ArgumentNullException">If the username or token is null this exception is thrown</exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if the data provided is not acceptable by the Authware API, the hardware ID did not match (if
    ///     enabled), the
    ///     application version is out-of-date (if enabled) or the username and password are invalid
    /// </exception>
    public async Task<BaseResponse> RedeemTokenAsync(string username, string token)
    {
        _ = _applicationId ?? throw new Exception($"{nameof(_applicationId)} can not be null");
        _ = username ?? throw new ArgumentNullException(username, $"{nameof(username)} can not be null");
        _ = token ?? throw new ArgumentNullException(token, $"{nameof(token)} can not be null");

        var response = await _requester
            .Request<BaseResponse>(HttpMethod.Post, "/user/renew",
                new { app_id = _applicationId, username, token })
            .ConfigureAwait(false);

        return response;
    }

    /// <summary>
    ///     Gets the currently authenticated users' profile
    /// </summary>
    /// <param name="authToken">The user's authentication token</param>
    /// <param name="isApiKey">Whether the provided auth token is an API key or not</param>
    /// <returns>The currently authenticated users' profile, represented as <see cref="Profile" /></returns>
    /// <exception cref="Exception">
    ///     This gets thrown if the application ID is null which would be if
    ///     <see cref="InitializeApplicationAsync" /> hasn't been called
    /// </exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if no user is authenticated
    /// </exception>
    public async Task<Profile> GetUserProfileAsync(string? authToken, bool isApiKey = false)
    {
        var _ = _applicationId ?? throw new Exception($"{nameof(_applicationId)} can not be null");
        return await _requester.Request<Profile>(HttpMethod.Get, "user/profile", null, authToken, isApiKey)
            .ConfigureAwait(false);
    }

    /// <summary>
    ///     Allows a user to change their email on your application to a new email
    /// </summary>
    /// <param name="authToken">The user's authentication token</param>
    /// <param name="password">The user's current password</param>
    /// <param name="email">The email the user wants to change their email to</param>
    /// <param name="isApiKey">Whether the provided auth token is an API key or not</param>
    /// <returns>A <see cref="BaseResponse" /> containing the code and the message returned from the authware api</returns>
    /// <exception cref="Exception">
    ///     This gets thrown if the application id is null which would be if
    ///     <see cref="InitializeApplicationAsync" /> hasn't been called
    /// </exception>
    /// <exception cref="ArgumentNullException">Throws if either password or email is null</exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if the data provided is not acceptable by the Authware API, the hardware ID did not match (if
    ///     enabled), the
    ///     application version is out-of-date (if enabled) or the password is invalid
    /// </exception>
    public async Task<BaseResponse> ChangeEmailAsync(string? authToken, string password, string email,
        bool isApiKey = false)
    {
        _ = _applicationId ?? throw new Exception($"{nameof(_applicationId)} can not be null");
        _ = password ?? throw new ArgumentNullException(password, $"{nameof(password)} can not be null");
        _ = email ?? throw new ArgumentNullException(email, $"{nameof(email)} can not be null");

        var response = await _requester
            .Request<BaseResponse>(HttpMethod.Put, "/user/change-email",
                new { password, new_email_address = email }, authToken, isApiKey)
            .ConfigureAwait(false);
        return response;
    }

    /// <summary>
    ///     Allows a user to change their current password to the password specified in newPassword
    /// </summary>
    /// <param name="authToken">The user's authentication token</param>
    /// <param name="currentPassword">The user's current password</param>
    /// <param name="newPassword">The password the user wants to change their password to</param>
    /// <param name="isApiKey">Whether the provided auth token is an API key or not</param>
    /// <returns>A <see cref="BaseResponse" /> containing the code and the message returned from the authware api</returns>
    /// <exception cref="Exception">
    ///     This gets thrown if the application id is null which would be if
    ///     <see cref="InitializeApplicationAsync" /> hasn't been called
    /// </exception>
    /// <exception cref="ArgumentNullException">Throws if either currentPassword or newPassword is null</exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if the data provided is not acceptable by the Authware API, the hardware ID did not match (if
    ///     enabled), the
    ///     application version is out-of-date (if enabled) or the password is invalid
    /// </exception>
    public async Task<BaseResponse> ChangePasswordAsync(string? authToken, string currentPassword, string newPassword,
        bool isApiKey = false)
    {
        _ = _applicationId ?? throw new Exception($"{nameof(_applicationId)} can not be null");
        _ = currentPassword ??
            throw new ArgumentNullException(currentPassword, $"{nameof(currentPassword)} can not be null");
        _ = newPassword ?? throw new ArgumentNullException(newPassword, $"{nameof(newPassword)} can not be null");

        var response = await _requester
            .Request<BaseResponse>(HttpMethod.Put, "/user/change-password",
                new
                {
                    old_password = currentPassword, password = newPassword,
                    repeat_password = newPassword
                }, authToken, isApiKey)
            .ConfigureAwait(false);
        return response;
    }

    /// <summary>
    ///     Executes a specific API under the current user
    /// </summary>
    /// <param name="authToken">The user's authentication token</param>
    /// <param name="apiId">The ID of the API to execute</param>
    /// <param name="parameters">The user-specified parameters to passthrough to the API</param>
    /// <param name="isApiKey">Whether the provided auth token is an API key or not</param>
    /// <returns>
    ///     The response given by your API, find the plaintext response under the 'DecodedResponse' property, the response
    ///     will be a status code if the 'Show API responses' setting is off
    /// </returns>
    /// <exception cref="Exception">
    ///     This gets thrown if the application id is null which would be if
    ///     <see cref="InitializeApplicationAsync" /> hasn't been called
    /// </exception>
    /// <exception cref="ArgumentNullException">Throws if the API ID is null</exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if the data provided is not acceptable by the Authware API, the hardware ID did not match (if
    ///     enabled), the
    ///     application version is out-of-date (if enabled), the API does not exist or the user does not have the required role
    ///     to execute it and if the API execution was not successful.
    /// </exception>
    public async Task<ApiResponse> ExecuteApiAsync(string? authToken, string apiId,
        Dictionary<string, object> parameters, bool isApiKey = false)
    {
        _ = _applicationId ?? throw new Exception($"{nameof(_applicationId)} can not be null");
        _ = apiId ?? throw new ArgumentNullException(apiId, $"{nameof(apiId)} can not be null");

        var apiResponse =
            await _requester.Request<ApiResponse>(HttpMethod.Post, "/api/execute", new { api_id = apiId, parameters },
                    authToken, isApiKey)
                .ConfigureAwait(false);
        return apiResponse;
    }

    /// <summary>
    ///     Allows a user to regenerate their API key depending on whether the functionality is enabled in your application
    /// </summary>
    /// <param name="authToken">The user's authentication token</param>
    /// <param name="password">The user's current password</param>
    /// <param name="isApiKey">Whether the provided auth token is an API key or not</param>
    /// <returns>A <see cref="BaseResponse" /> containing the code and the message returned from the Authware API</returns>
    /// <exception cref="Exception">
    ///     This gets thrown if the application id is null which would be if
    ///     <see cref="InitializeApplicationAsync" /> hasn't been called
    /// </exception>
    /// <exception cref="ArgumentNullException">Throws if the password is null</exception>
    /// <exception cref="AuthwareException">
    ///     Thrown if the data provided is not acceptable by the Authware API, the hardware ID did not match (if enabled), the
    ///     application version is out-of-date (if enabled) or the password is invalid
    /// </exception>
    public async Task<BaseResponse> RegenerateApiKeyAsync(string authToken, string password, bool isApiKey = false)
    {
        _ = _applicationId ?? throw new Exception($"{nameof(_applicationId)} can not be null");
        _ = password ?? throw new ArgumentNullException(password, $"{nameof(password)} can not be null");

        var response = await _requester
            .Request<BaseResponse>(HttpMethod.Put, "/user/regenerate-key", new { password }, authToken, isApiKey)
            .ConfigureAwait(false);

        return response;
    }
}