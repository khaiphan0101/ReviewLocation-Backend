using CloudinaryDotNet.Actions;
using review.Common.Entities;
using review.Common.Helpers;
using review.Common.ReqModels;
using review.Controllers;
using review.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using review.Common.ResModels;
using Microsoft.AspNetCore.Http;
using review.Common.Constatnts;
using System.Reflection;

namespace review.Services
{
    public interface IUserProfileService
    {
        Task<ApiResModel> UpdateUserProfile(UserProfileReqModel req, string id);

        Task<ApiResModel> ChangePassword(ChangePasswordReqModel req, string id);

        Task<ApiResModel> Follow(string userId, string id);

        Task<ApiResModel> UnFollow(string userId, string id);

        Task<ApiResModel> GetMyFollowCountInfo(string id);

        Task<ApiResModel> GetProfile(string id);

        Task<PagingResModel> MyFollowerInfo(string id, int page);

        Task<PagingResModel> MyFollowingInfo(string id, int page);
    }

    public class UserProfileService : IUserProfileService
    {
        private readonly DataContext _dataContext;
        private readonly ICloudinaryService _cloudinaryService;

        public UserProfileService(DataContext dataContext, ICloudinaryService cloudinaryService)
        {
            _dataContext = dataContext;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ApiResModel> UpdateUserProfile(UserProfileReqModel req, string id)
        {
            try
            {
                
                var userProfile = _dataContext.ProfileEntitys.FirstOrDefault(u => u.AccountID == id); 
                if (userProfile == null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "User is not exist!"
                    };
                }
                ImageUploadResult image = new ImageUploadResult();
                if (req.Avatar is not null) 
                { 
                    image = await _cloudinaryService.UploadImage(req.Avatar);
                    if(userProfile.Avatar != null)
                    {
                        await _cloudinaryService.DeleteImage(userProfile.Avatar);
                    }    
                }
                userProfile.Name = req.Name is not null ? req.Name : userProfile.Name;
                userProfile.Phone = req.Phone is not null ? req.Phone : userProfile.Phone;
                userProfile.Gender = req.Gender is not null ? req.Gender : userProfile.Gender;
                userProfile.Avatar = image.PublicId is not null ? image.PublicId : userProfile.Avatar;
                _dataContext.ProfileEntitys.Update(userProfile);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel()
                {
                    StatusCode = 200,
                    MessageText = "Updated user profile successfully!",
                    Data = userProfile,
                };
            }           
            catch(Exception ex)
            {
                return new ApiResModel()
                {
                    StatusCode = 400,
                    MessageText = "Some thing went wrong! Please try again!",
                    ErrorText = ex.Message
                };
            }

        }
        public async Task<ApiResModel> ChangePassword(ChangePasswordReqModel req, string id)
        {
            try
            {
                var userAccount = _dataContext.AccountEntitys.FirstOrDefault(u => u.ID == id);
                if(userAccount == null)
                {
                    return new ApiResModel() { StatusCode = 400, MessageText = "User is not exits!" };
                }
                if(!PasswordHelper.VerifyHashedPassword(userAccount.Password, req.OldPassword))
                {
                    return new ApiResModel() { StatusCode = 400, MessageText = "OldPassword is not correct! Please try again!" };
                }
                if (req.NewPassword == req.OldPassword)
                {
                    return new ApiResModel() { StatusCode = 400, MessageText = "Please typing other new Password!" };
                }
                if (req.ConfirmPassword != req.NewPassword)
                {
                    return new ApiResModel() { StatusCode = 400, MessageText = "ConfirmPassword is not correct! Please try again!" };
                }
                var hash = PasswordHelper.HashPassword(req.NewPassword);
                userAccount.Password = hash;
                _dataContext.AccountEntitys.Update(userAccount);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel() { StatusCode = 200, MessageText = "Password changed successfully!" };
            }
            catch (Exception ex)
            {
                return new ApiResModel() { StatusCode = 400, MessageText = "Something went wrong!", ErrorText = ex.Message };
            }
        }

        public async Task<ApiResModel> Follow(string userId, string id)
        {
            try
            {
                var followerProfile = _dataContext.ProfileEntitys.FirstOrDefault(u => u.AccountID == userId);//my profile info
                var followingProfile = _dataContext.ProfileEntitys.FirstOrDefault(u => u.AccountID == id);//other info who i will follow
                if (userId == id)
                {
                    return new ApiResModel() { StatusCode = 400, MessageText = "You can't follow your selft!" };
                }
                if (followingProfile == null)
                {
                    return new ApiResModel() { StatusCode = 400, MessageText = "User you try to follow is not exits!" };
                }
                var followInfo = _dataContext.ProfileFollowEntitys.FirstOrDefault(f => f.FollowerID == followerProfile.ID && f.FollowingID == followingProfile.ID);
                if (followInfo != null)
                {
                    return new ApiResModel() { StatusCode = 400, MessageText = "You already follow this user!" };
                }
                ProfileFollowEntity data = new ProfileFollowEntity()
                {
                    ID = Guid.NewGuid().ToString(),
                    FollowerID = followerProfile.ID,
                    FollowingID = followingProfile.ID
                };
                _dataContext.ProfileFollowEntitys.Add(data);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel() { StatusCode = 200, MessageText = "Follow successful!" };
            }
            catch (Exception ex)
            {
                return new ApiResModel() { StatusCode = 400, MessageText = "Some thing went wrong! Please try again!", ErrorText = ex.Message };
            }
        }

        public async Task<ApiResModel> UnFollow(string userId, string id)
        {
            try
            {
                var followerProfile = _dataContext.AccountEntitys.Include(u => u.Profile).FirstOrDefault(u => u.ID == userId);//my profile info
                var followingProfile = _dataContext.AccountEntitys.Include(u => u.Profile).FirstOrDefault(u => u.ID == id);//other info who i will follow
                if (followingProfile == null || followerProfile == null)
                {
                    return new ApiResModel() { StatusCode = 400, MessageText = "User you try to unfollow is not exits!" };
                }
                var followInfo = _dataContext.ProfileFollowEntitys.FirstOrDefault(f => f.FollowerID == followerProfile.Profile.ID && f.FollowingID == followingProfile.Profile.ID);
                if (followInfo == null)
                {
                    return new ApiResModel() { StatusCode = 400, MessageText = "You not follow this user!" };
                }
                _dataContext.ProfileFollowEntitys.Remove(followInfo);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel() { StatusCode = 200, MessageText = "UnFollow successful!" };
            }
            catch (Exception ex)
            {
                return new ApiResModel() { StatusCode = 400, MessageText = ex.Message, ErrorText = ex.Message };
            }
        }

        public async Task<ApiResModel> GetMyFollowCountInfo(string id)
        {
            try
            {
                var userData = _dataContext.AccountEntitys.Include(a => a.Profile).FirstOrDefault(a => a.ID == id);
                var followerData = _dataContext.ProfileFollowEntitys.Where(p => p.FollowingID == userData.Profile.ID).Include(p => p.Follower).ToList();
                var followingData = _dataContext.ProfileFollowEntitys.Where(p => p.FollowerID == userData.Profile.ID).Include(p => p.Following).ToList();

                return new ApiResModel()
                { 
                    StatusCode = 200,
                    MessageText = "Get successful!",
                    Data = new {
                        followerCount = followerData.Count(),
                        followingCount = followingData.Count(),
                    }
                    
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel() { StatusCode = 400, MessageText = "Something went wrong! Please try again!", ErrorText = ex.Message };
            }
        }

        public async Task<PagingResModel> MyFollowerInfo(string id, int page)
        {
            try
            {
                var userData = _dataContext.AccountEntitys.Include(a => a.Profile).FirstOrDefault(a => a.ID == id);
                var followerData = _dataContext.ProfileFollowEntitys.Where(p => p.FollowingID == userData.Profile.ID).Include(p => p.Follower).ToList();
                var followerList = followerData.Select(f => new
                {
                    f.Follower.ID,
                    f.Follower.Name,
                    f.Follower.Phone,
                    f.Follower.Gender,
                    f.Follower.Identify,
                    f.Follower.AccountID,
                }).Skip((page - 1) * PagingConstant.PageSize).Take(PagingConstant.PageSize);
                return new PagingResModel()
                {
                    StatusCode = 200,
                    MessageText = "Get successful!",
                    Data = new { followerList },
                    TotalData = followerData.Count(),
                    CurrentPage = page,
                    TotalPage = Math.Ceiling((decimal)followerData.Count()/ PagingConstant.PageSize)

                };
            }
            catch (Exception ex)
            {
                return new PagingResModel() { StatusCode = 400, MessageText = "Something went wrong! Please try again!", ErrorText = ex.Message };
            }
        }

        public async Task<PagingResModel> MyFollowingInfo(string id, int page)
        {
            try
            {
                var userData = _dataContext.AccountEntitys.Include(a => a.Profile).FirstOrDefault(a => a.ID == id);
                var followingData = _dataContext.ProfileFollowEntitys.Where(p => p.FollowerID == userData.Profile.ID).Include(p => p.Following).ToList();
                var followingList = followingData.Select(f => new
                {
                    f.Following.ID,
                    f.Following.Name,
                    f.Following.Phone,
                    f.Following.Gender,
                    f.Following.Identify,
                    f.Following.AccountID,
                }).Skip((page - 1) * PagingConstant.PageSize).Take(PagingConstant.PageSize); ;
                return new PagingResModel()
                {
                    StatusCode = 200,
                    MessageText = "Get successful!",
                    Data = new { followingList },
                    TotalData = followingData.Count(),
                    CurrentPage = page,
                    TotalPage = Math.Ceiling((decimal)followingData.Count() / PagingConstant.PageSize)

                };
            }
            catch (Exception ex)
            {
                return new PagingResModel() { StatusCode = 400, MessageText = "Something went wrong! Please try again!", ErrorText = ex.Message };
            }
        }

        public async Task<ApiResModel> GetProfile(string userId)
        {
            try
            {
                var account = _dataContext.AccountEntitys.Include(a => a.Profile).FirstOrDefault(u => u.ID == userId);
                
                return new ApiResModel() 
                { 
                    StatusCode = 200,
                    MessageText = "successful!", 
                    Data = new ProfileEntity()
                    {
                        ID = account.Profile.ID,
                        Name = account.Profile.Name,
                        Phone = account.Profile.Phone,
                        Gender = account.Profile.Gender,
                        Avatar = $"https://res.cloudinary.com/dbey8svpl/image/upload/v1696056834/{account.Profile.Avatar}.jpg",
                        Identify = account.Profile.Identify,
                        AccountID = account.Profile.AccountID,
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel() { StatusCode = 400, MessageText = "Some thing went wrong! Please try again!", ErrorText = ex.Message };
            }
        }
    }
}
