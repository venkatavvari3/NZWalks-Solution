using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ItokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
