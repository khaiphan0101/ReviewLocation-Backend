using Microsoft.Extensions.Options;
using review.Common;
using review.Common.Entities;
using review.Common.Helpers;
using review.Common.Models;
using review.Common.ReqModels;
using review.Data;
using System;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using review.Common.ResModels;
using Microsoft.AspNetCore.Http;
using review.Common.Constatnts;

namespace review.Services
{
    public interface IAuthService
    {
        Task<ApiResModel> SignUp(SignUpReqModel req);
        Task<ApiResModel> SignIn(SignInReqModel req);

        Task<ApiResModel> RefeshToken(string refeshToken);
    }
    public class AuthService : IAuthService
    {
        private readonly DataContext _dataContext;
        private readonly JwtTokensOptionsModel _jwtTokensOptions;
        public AuthService(DataContext dataContext, IOptions<JwtTokensOptionsModel> jwtTokensOptions) 
        {
            _dataContext = dataContext;
            _jwtTokensOptions = jwtTokensOptions.Value;
        }

        public async Task<ApiResModel> RefeshToken(string refeshToken)
        {
            try
            {
                //"Include" dùng để truy vấn lấy các thông tin mà bảng này có quan hệ với bảng khác 
                //ví dụ bảng rftoken có quan hệ với bảng account thì dùng include theo cú pháp bên dưới để lấy luôn thông tin 
                //account cho cái rftoken này để tọa accesstoken mới 
                var token = _dataContext.RefeshTokenEntitys.Include(tk => tk.Account).FirstOrDefault(tk => tk.RefeshToken == refeshToken);
                if (token == null || DateTime.Compare(token.ExpiredAt, DateTime.Now) < 0)//nếu không tồn tại rftoken hoặc rftoken hết hạn
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "Hết phiên đăng nhập! Hãy đăng nhập lại!",
                    };
                }
                //Tạo accesstoken mới
                TokenModel newToken = new JwtTokensHelper(_jwtTokensOptions).GenerateJSONWebToken(token.Account, false);

                return new ApiResModel()
                    {
                        StatusCode = 200,
                        MessageText = "Success!",
                        Data = newToken
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel()
                {
                    StatusCode = 400,
                    MessageText = "Something went wrong. Please try again!",
                    ErrorText = ex.Message
                };
            }
            
        }

        public async Task<ApiResModel> SignIn(SignInReqModel req)
        {
            try
            {
                var user = _dataContext.AccountEntitys.FirstOrDefault(u => u.UserName == req.UserName || u.Email == req.UserName);
                if (user == null)
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "UserName or Email not exist!",
                    };
                }
                if (!PasswordHelper.VerifyHashedPassword(user.Password, req.Password))//giải mã mật khẩu 
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "Wrong Password!",
                    };
                }
                //Tạo token
                TokenModel token = new JwtTokensHelper(_jwtTokensOptions).GenerateJSONWebToken(user);
                //Lưu refesh token
                var refeshToken = new RefeshTokenEntity()
                {
                    ID = Guid.NewGuid().ToString(),
                    RefeshToken = token.RefeshToken,
                    ExpiredAt = DateTime.Now.AddDays(30),//ngày hiện tại cộng thêm 30 ngày
                    Account = user//tự động lưu account id khi lưu rftoken
                };
                _dataContext.RefeshTokenEntitys.Add(refeshToken);
                await _dataContext.SaveChangesAsync();

                return new ApiResModel() { StatusCode = 200, MessageText = "Success!", Data = token };
            }
            catch (Exception ex)
            {
                return new ApiResModel()
                {
                    StatusCode = 400,
                    MessageText = "Something went wrong. Please try again!",
                    ErrorText = ex.Message
                };
            }
        }

        public async Task<ApiResModel> SignUp(SignUpReqModel req)
        {
            try
            {
                var user = _dataContext.AccountEntitys.FirstOrDefault(u => u.UserName == req.UserName || u.Email == req.Email);
                if (user != null) 
                {
                    return new ApiResModel()
                    {
                        StatusCode = 400,
                        MessageText = "UserName Or Email already exist!",
                    };
                } 
                AccountEntity account = new AccountEntity()
                {
                    ID = Guid.NewGuid().ToString(),
                    UserName = req.UserName,
                    Role = RolesConstant.Admin,
                    Password = PasswordHelper.HashPassword(req.Password),//mã hóa mật khẩu 
                    Email = req.Email,
                    Profile = new ProfileEntity() //Tạo profile mặc định
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = null,
                        Phone = null,
                        Gender = null,
                        Avatar = "https://res.cloudinary.com/dbey8svpl/image/upload/v1694912519/user_tidw2g.png",
                        Identify = $"@{req.UserName}.{DateTime.Now.Month}{DateTime.Now.Year}"
                    }
                };
                _dataContext.AccountEntitys.Add(account);
                await _dataContext.SaveChangesAsync();
                return new ApiResModel()
                {
                    StatusCode = 200,
                    MessageText = "create account success!",
                };
            }
            catch (Exception ex)
            {
                return new ApiResModel
                {
                    StatusCode = 400,
                    MessageText = "Something went wrong. Please try again!",
                    ErrorText = ex.Message,
                };
            }
        }
    }
}
