
using CareerSpotlightBackend.Data;
using Microsoft.EntityFrameworkCore;

namespace CareerSpotlightBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure DbContext with detailed logging for debugging
            var optionsBuilder = new DbContextOptionsBuilder<CareerSpotlightContext>();
            optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
                          .EnableSensitiveDataLogging()
                          .LogTo(Console.WriteLine, LogLevel.Information);

            builder.Services.AddSingleton(new CareerSpotlightContext(optionsBuilder.Options));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
