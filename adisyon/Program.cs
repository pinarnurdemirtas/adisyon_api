using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using adisyon.Data;
using adisyon.Helpers;
using Microsoft.Extensions.Options;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Bağlantı dizesini appsettings.json'dan alıyoruz
builder.Services.AddDbContext<AdisyonDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));


// JwtSettings'i alıyoruz
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// JWT kimlik doğrulama ekliyoruz
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,  // İstenirse issuer doğrulaması eklenebilir
            ValidateAudience = false,  // İstenirse audience doğrulaması eklenebilir
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero // Token'ın süresi dolduğunda hemen geçersiz sayılacak
        };
    });

// Diğer servisleri ekliyoruz
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger ve middleware kullanımı
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();  // JWT kimlik doğrulama ekliyoruz
app.UseAuthorization();

app.MapControllers();

app.Run();
