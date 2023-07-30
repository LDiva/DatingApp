using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class LikesRepository : ILikeRepository
    {
        private readonly DataContext _context;
        public LikesRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<UserLike> GetUserLike(int sourceUserID, int targetUserID)
        {
            return await _context.Likes.FindAsync(sourceUserID, targetUserID);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes =_context.Likes.AsQueryable();

            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserID == likesParams.UserID);
                users = likes.Select(like => like.TargetUser);
            }

            
            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.TargetUserID == likesParams.UserID);
                users = likes.Select(like => like.SourceUser);
            }

            var LikedUsers = users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                KnownAs =user.KnownAs,
                Age = user.DateOfBirth.CalcuateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,
                ID = user.ID
            });

            return await PagedList<LikeDto>.CreateAsync(LikedUsers, likesParams.PageNumber,likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikes(int userID)
        {
            return await _context.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.ID == userID);
        }
    }
}