using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using UniversityRegistration.Api.Data;
using UniversityRegistration.Api.Helpers;
using UniversityRegistration.Api.Repository.Implementations;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Implementations;
using UniversityRegistration.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repository
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

// Services
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICourseService, CourseService>();

// JWT Helper
var secretKey = builder.Configuration["JwtSettings:Secret"]!;
builder.Services.AddSingleton(new JwtHelper(secretKey));

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

// Swagger
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Run Seeder (after Build)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();     // auto-create DB
    DbSeeder.SeedAdmins(db);  // seed admin
}

// Middleware
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
