using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update (AppUser user);

       Task<bool> SaveAllAsync();

       Task<IEnumerable<AppUser>> GetUsersAsync();

       Task<AppUser> GetUserByIdAsync(int Id);

       Task<AppUser> GetUserByUsernameAsync(string username);

       Task<PagedList<MemberDto>> GetMembersAsync(UserParms userParms);

       Task<MemberDto> GetMemberAsync(string username);
        
        
        
    }
}