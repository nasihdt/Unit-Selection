using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UniversityRegistration.Api.Data;
using UniversityRegistration.Api.Helpers;
using UniversityRegistration.Api.Repository.Implementations;
using UniversityRegistration.Api.Repository.Interfaces;
using UniversityRegistration.Api.Services.Implementations;
using UniversityRegistration.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ================================
// Controllers
// ================================
builder.Services.AddControllers();

// ================================
// CORS
// ================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ================================
// DbContext
// ================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// ================================
// Repositories
// ================================
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IProfessorRepository, ProfessorRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IRegistrationSettingsRepository, RegistrationSettingsRepository>();
builder.Services.AddScoped<ICoursePrerequisiteRepository, CoursePrerequisiteRepository>();

// ================================
// Services
// ================================
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IProfessorService, ProfessorService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IRegistrationSettingsService, RegistrationSettingsService>();
builder.Services.AddScoped<ICoursePrerequisiteService, CoursePrerequisiteService>();

// ================================
// JWT Helper
// ================================
var secretKey = builder.Configuration["JwtSettings:Secret"]!;
builder.Services.AddSingleton(new JwtHelper(secretKey));

// ================================
// Authentication (JWT)
// ================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey)
            ),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// ================================
// Swagger + JWT
// ================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT token like: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ================================
// Migration + Seed
// ================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    DbSeeder.SeedAdmins(db);
    DbSeeder.SeedStudents(db);
    DbSeeder.SeedProfessors(db);
}

// ================================
// Middlewares
// ================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
