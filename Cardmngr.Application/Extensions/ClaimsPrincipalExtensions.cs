using System.Security.Claims;

namespace Cardmngr.Application.Extensions;

public static class ClaimsPrincipalExtensions
{
    /// <exception cref="NoNameIdentifierClaimFoundException"></exception>
    public static string GetNameIdentifier(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? throw new NoNameIdentifierClaimFoundException();
}
