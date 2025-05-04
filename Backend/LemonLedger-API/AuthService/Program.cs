using AuthService.Data;
using AuthService.Interfaces;
using AuthService.Models;
using AuthService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1) CORS
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("AllowLocalhost3000", policy =>
        policy.WithOrigins(
                "http://localhost:3000",
                "https://localhost:3000",
                "https://dora-e-fshatit.vercel.app"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

// 2) DbContext (MySQL)
var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AuthDbContext>(opts =>
    opts.UseMySql(conn, ServerVersion.AutoDetect(conn))
);

// 3) Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
{
    opts.User.RequireUniqueEmail = true;
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = false;
})
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// 4) JwtSettings binding
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

// 5) JWT Authentication
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        var jwt = builder.Configuration
                         .GetSection("JwtSettings")
                         .Get<JwtSettings>();

        var signingKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt.Key)
                     );

        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey
        };
    });

// 6) Application services
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<IAuthService, AuthService.Services.AuthService>();

// 7) MVC & Swagger         
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 8) Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost3000");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
