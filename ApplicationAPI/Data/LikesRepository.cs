

using ApplicationAPI.DTOs;
using ApplicationAPI.Entities;
using ApplicationAPI.Extensions;
using ApplicationAPI.Helpers;
using ApplicationAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApplicationAPI.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _db;
        public LikesRepository(DataContext db)
        {
            _db = db;            
        }
        public async Task<UserLike> GetUserLike(int sourceUserId, int targetUserId)
        {
            return await _db.Likes.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _db.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _db.Likes.AsQueryable();

            if(likesParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.TargetUser);
            }  
            if(likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.TargetUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }  
            var likedUsers = users.Select(user => new LikeDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,
                Id = user.Id
            }); 

            return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);          
        }

        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _db.Users
                .Include(x => x.LikedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}