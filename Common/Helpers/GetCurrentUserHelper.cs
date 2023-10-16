using System.Security.Claims;

namespace review.Common.Helpers
{
    public class GetCurrentUserHelper
    {
        public static string GetUserID(HttpContext _httpContext)
        {
            var identity = _httpContext.User.Identity as ClaimsIdentity;
            var userClaims = identity.Claims;
            var userData = new { id = userClaims.FirstOrDefault(x => x.Type == "id")?.Value };
            return userData.id;
        }
    }
}
