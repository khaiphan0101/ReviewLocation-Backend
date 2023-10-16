using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using review.Common.Entities;
using review.Common.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace review.Common.Helpers
{
    public class JwtTokensHelper
    {
        private readonly JwtTokensOptionsModel _jwtTokensOptions;

        public JwtTokensHelper(JwtTokensOptionsModel jwtTokensOptions)
        {
            _jwtTokensOptions = jwtTokensOptions;
        }

        public TokenModel GenerateJSONWebToken(AccountEntity user, bool gRefeshToken = true)
        {
            //tạo thông tin cần lưu vào token
            var claims = new[]
            {
                new Claim("userName", user.UserName),
                new Claim("role", user.Role),
                new Claim("email", user.Email),
                new Claim("id", user.ID)
            };
            //chữ ký bí mật để mã hóa tạo token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtTokensOptions.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //tạo thời gian sống của token
            DateTime expires = DateTime.Now.AddMinutes(_jwtTokensOptions.ExpireAfterMinutes);

            var securityToken = new JwtSecurityToken(_jwtTokensOptions.Issuer,
                                                     _jwtTokensOptions.Audience,
                                                     claims,
                                                     expires: expires,
                                                     signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            var refreshToken = "";
            if (gRefeshToken)
            {
                refreshToken = GenerateRefreshToken();
            }
            return new TokenModel
            {
                AccessToken = token,
                RefeshToken = refreshToken,
            };
        }

        public string GenerateRefreshToken()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
