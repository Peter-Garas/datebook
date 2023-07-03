

using ApplicationAPI.Entities;

namespace ApplicationAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}