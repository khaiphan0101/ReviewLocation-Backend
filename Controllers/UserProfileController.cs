using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using review.Common.Helpers;
using review.Common.ReqModels;
using review.Services;
using System.Security.Claims;

namespace review.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]//khai báo controller này cần đăng nhập nhưng không cần phân quyền
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UserProfileReqModel req)
        {

            string id = GetCurrentUserHelper.GetUserID(HttpContext);
            var response = await _userProfileService.UpdateUserProfile(req, id);
            return Ok(response);
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordReqModel req) 
        {
            string id = GetCurrentUserHelper.GetUserID(HttpContext);
            var response = await _userProfileService.ChangePassword(req, id);
            return Ok(response);
        }
        //all route follow xét theo ID trong tài Account
        [HttpPost("follow/{id}")]
        public async Task<IActionResult> Follow(string id)
        {
            string userId = GetCurrentUserHelper.GetUserID(HttpContext);
            var response = await _userProfileService.Follow(userId, id);
            return Ok(response);
        }

        [HttpDelete("unFollow/{id}")]
        public async Task<IActionResult> UnFollow(string id)
        {
            string userId = GetCurrentUserHelper.GetUserID(HttpContext);
            var response = await _userProfileService.UnFollow(userId, id);
            return Ok(response);
        }

        [HttpGet("followCountInfo")]
        public async Task<IActionResult> MyFollowCountInfo()
        {
            //chỉ cần lấy id của user trong token để lấy thông tin follow
            string userId = GetCurrentUserHelper.GetUserID(HttpContext);
            var response = await _userProfileService.GetMyFollowCountInfo(userId);
            return Ok(response);
        }

        [HttpGet("followerInfo")]//lay thong tin nguoi dang theo doi minh
        public async Task<IActionResult> MyFollowerInfo(int page = 1)
        {
            //chỉ cần lấy id của user trong token để lấy thông tin follow
            string userId = GetCurrentUserHelper.GetUserID(HttpContext);
            var response = await _userProfileService.MyFollowerInfo(userId, page);
            return Ok(response);
        }

        [HttpGet("followingInfo")]//lay thong tin nguoi minh dang theo doi
        public async Task<IActionResult> MyFollowingInfo(int page = 1)
        {
            //chỉ cần lấy id của user trong token để lấy thông tin follow
            string userId = GetCurrentUserHelper.GetUserID(HttpContext);
            var response = await _userProfileService.MyFollowingInfo(userId, page);
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            //chỉ cần lấy id của user trong token để lấy thông tin follow
            string userId = GetCurrentUserHelper.GetUserID(HttpContext);
            var response = await _userProfileService.GetProfile(userId);
            return Ok(response);
        }
    }
}
