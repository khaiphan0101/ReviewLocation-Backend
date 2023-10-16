using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Text;
using NetCore.AutoRegisterDi;
using review.Common;
using review.Data;
using Microsoft.EntityFrameworkCore;
using review.Common.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Config Auth
//đọc dữ liệu từ appsetting.json
var jwtTokensOption = builder.Configuration.GetSection("JwtTokens");
var cloudinaryOption = builder.Configuration.GetSection("Cloudinary");
builder.Services.Configure<JwtTokensOptionsModel>(jwtTokensOption);
builder.Services.Configure<CloudinaryOptionsModel>(cloudinaryOption);
//khai bào phương thức xác thực là Bearer
builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
        .RequireAuthenticatedUser().Build());
});
//config cho jwt
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtTokensOption.GetValue<string>(nameof(JwtTokensOptionsModel.Issuer)),
            ValidAudience = jwtTokensOption.GetValue<string>(nameof(JwtTokensOptionsModel.Audience)),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokensOption.GetValue<string>(nameof(JwtTokensOptionsModel.SigningKey)))),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

#endregion Config Auth

//DI tất cả các file kết thúc bằng Service sẽ được khai báo dependency
builder.Services.AddScoped<DataContext, DataContext>();
builder.Services.AddDbContext<DataContext>(options =>
     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.RegisterAssemblyPublicNonGenericClasses()
        .Where(c => c.Name.EndsWith("Service"))
        .AsPublicImplementedInterfaces();

//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "local",
        policy =>
        {
            policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
        });
});

var app = builder.Build();

app.UseCors("local");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
