using Education.Entity.Models;

namespace Education.Business.Core.@abstract
{
    public interface ITokenService
    {
        Task<string> GenerateToken(ApplicationUser user);
    }
}
