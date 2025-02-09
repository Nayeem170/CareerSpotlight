using CareerSpotlightApi.Data;
using CareerSpotlightApi.Middlewares;
using CareerSpotlightApi.Models;
using CareerSpotlightApi.Models.Settings;
using CareerSpotlightApi.Providers;
using CareerSpotlightApi.Services;
using CareerSpotlightApi.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace CareerSpotlightApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
            var services = builder.Services;

            // Configure application settings
            services.Configure<TokenSettings>(configuration.GetSection("TokenSettings"));
            services.Configure<IdentitySettings>(configuration.GetSection("IdentitySettings"));
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            // Bind JWT settings
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Key ?? string.Empty);

            // Add controllers with JSON serialization settings
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
            });

            // Configure CORS policy
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("https://localhost:7076")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Add Swagger documentation with JWT authentication
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });
            });

            // Configure database context
            services.AddDbContext<CareerSpotlightContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging(); // For debugging only, disable in production
                options.LogTo(Console.WriteLine, LogLevel.Information);
            });

            // Configure Identity services
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<CareerSpotlightContext>()
                .AddTokenProvider<CustomTokenProvider<User>>("EmailVerification")
                .AddDefaultTokenProviders();

            // Configure JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            // Register application services
            services.AddSingleton<IJwtTokenService, JwtTokenService>();
            services.AddSingleton<IEmailService, EmailService>();

            // Configure logging
            using var loggerFactory = LoggerFactory.Create(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
            var logger = loggerFactory.CreateLogger<Program>();
            services.AddSingleton(logger);

            var app = builder.Build();

            // Configure middleware pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
