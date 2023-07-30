using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ILikeRepository 
    {
        Task<UserLike> GetUserLike(int sourceUserID, int targetUserID);
        Task<AppUser> GetUserWithLikes(int userID);
        Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);
        
    }
}