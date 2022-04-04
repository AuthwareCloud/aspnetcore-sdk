using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Principal;
using Authware.AspNetCore.Models;

namespace Authware.AspNetCore.Utils;

public sealed class AuthwareClaimsPrincipal : ClaimsPrincipal
{
    public AuthwareClaimsPrincipal(Profile profile)
    {
        Profile = profile;
    }

    public AuthwareClaimsPrincipal(IEnumerable<ClaimsIdentity> identities, Profile profile) : base(identities)
    {
        Profile = profile;
    }

    public AuthwareClaimsPrincipal(BinaryReader reader, Profile profile) : base(reader)
    {
        Profile = profile;
    }

    protected AuthwareClaimsPrincipal(SerializationInfo info, StreamingContext context, Profile profile) : base(info, context)
    {
        Profile = profile;
    }

    public AuthwareClaimsPrincipal(IIdentity identity, Profile profile) : base(identity)
    {
        Profile = profile;
    }

    public AuthwareClaimsPrincipal(IPrincipal principal, Profile profile) : base(principal)
    {
        Profile = profile;
    }

    public Profile Profile { get; set; }
}