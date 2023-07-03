

using ApplicationAPI.DTOs;
using ApplicationAPI.Entities;
using ApplicationAPI.Helpers;

namespace ApplicationAPI.Interfaces
{
    public interface IUserRepository
    {
         void Update(AppUser user);
         Task<IEnumerable<AppUser>> GetAppUsersAsync();
         Task<AppUser> GetUserByIdAsync(int id);
         Task<AppUser> GetUserByUsernameAsync(string username); 
         Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
         Task<MemberDto> GetMemberAsync(string username);
         Task<string> GetUserGender(string username);      
    }
}