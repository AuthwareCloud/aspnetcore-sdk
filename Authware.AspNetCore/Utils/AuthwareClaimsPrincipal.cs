namespace Authware.AspNetCore.Utils;

public sealed class AuthwareClaimsPrincipal : ClaimsPrincipal
{
    /// <summary>
    ///     The user profile associated with this claims principal
    /// </summary>
    public Profile Profile { get; }

    /// <summary>
    ///     Constructs the claims principal
    /// </summary>
    /// <param name="claims">The list of claims to use</param>
    /// <param name="profile">The user profile to associate the claims with</param>
    public AuthwareClaimsPrincipal(IEnumerable<Claim> claims, Profile profile)
    {
        var claimsIdentity = new ClaimsIdentity(claims, "authware");
        AddIdentity(claimsIdentity);

        Profile = profile;
    }
}