using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Principal;
using Authware.AspNetCore.Models;

namespace Authware.AspNetCore.Utils;

public sealed class AuthwareClaimsPrincipal : ClaimsPrincipal
{

    public Profile Profile { get; set; }

    public AuthwareClaimsPrincipal(IEnumerable<Claim> claims, Profile profile)
    {
        var claimsIdentity = new ClaimsIdentity(claims, "authware");
        base.AddIdentity(claimsIdentity);

        Profile = profile;
    }
}