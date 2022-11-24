using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Misa.AMIS.Auth.Common;

using Misa.AMIS.Auth.Model;
using Misa.AMIS.Auth.Models;
using Misa.AMIS.Auth.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
// Add services to the container.




//Connect DB
DataContext.AuthSqlConnectionString = builder.Configuration.GetConnectionString("AuthSqlConnectionString");

builder.Services.AddDbContext<AppliationDBContext>(options =>
{
    options.UseMySql(
           DataContext.AuthSqlConnectionString,
           new MySqlServerVersion(new Version(8, 0, 11))
       );
    //options.UseMySql(builder.Configuration.GetConnectionString("AuthSqlConnectionString"));
});

builder.Services.AddIdentity<ApplicationUserFull, IdentityRole>(options =>
{
    //quy định format password
    options.Password.RequireDigit = false;//Cần có số
    options.Password.RequireLowercase = false;//Cần chữ thường
    options.Password.RequireUppercase = false;//Cần chữ hoa
    options.Password.RequireNonAlphanumeric = false;//Cần ký tự
    options.Password.RequiredLength = 4;//min length

}).AddEntityFrameworkStores<AppliationDBContext>();
//tạo mã xác nhận email hoặc số điện thoại(thực hiện xác thực 2 yếu tố)

//Dependence Injection 
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IManagerUserService, ManagerUserService>();


////Add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{    
    o.TokenValidationParameters = new TokenValidationParameters
    {

      


        ValidIssuer = builder.Configuration[Resource.JwtIssuer],
        ValidAudience = builder.Configuration[Resource.JwtAudience],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),


        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true
    };
});



builder.Services.AddControllers();

//add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
                          policy =>
                          {
                              policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                          });
});


//builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();// luôn luôn phải đứng trên author

app.UseAuthorization();

app.MapControllers();

app.Run();
