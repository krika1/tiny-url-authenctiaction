using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using TinyUrl.AuthenticationService.Bussiness.Services;
using TinyUrl.AuthenticationService.Data.Repositories;
using TinyUrl.AuthenticationService.Infrastructure.Context;
using TinyUrl.AuthenticationService.Infrastructure.Contracts.Options;
using TinyUrl.AuthenticationService.Infrastructure.Repositories;
using TinyUrl.AuthenticationService.Infrastructure.Services;


namespace TinyUrl.AuthenticationService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<MongoDbOptions>(
                builder.Configuration.GetSection("MongoDbOptions"));

            builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<MongoDbOptions>>().Value;
                return new MongoClient(options.ConnectionString);
            });


            builder.Services.AddScoped<MongoDbContext>();

            builder.Services.AddScoped<IAuthenticationService, Bussiness.Services.AuthenticationService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                            .AddJwtBearer(options =>
                            {
                                options.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateLifetime = true,
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq")),
                                    ValidateIssuer = false,
                                    ValidateAudience = false
                                };

                                options.Events = new JwtBearerEvents
                                {
                                    OnAuthenticationFailed = context =>
                                    {
                                        Console.WriteLine("Token failed validation: " + context.Exception.Message);
                                        return Task.CompletedTask;
                                    }
                                };
                            });

            var app = builder.Build();

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
        }
    }
}
