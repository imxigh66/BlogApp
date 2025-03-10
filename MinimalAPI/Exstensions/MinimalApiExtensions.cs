using Application.Abstractions;
using Application.Posts.Commands;
using DataAccess.Repositories;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MinimalAPI.Exstensions
{
    public static class MinimalApiExtensions
    {

        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            var cs = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<BlogDbContext>(opt => opt.UseSqlServer(cs));


            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<ILikeRepository, LikeRepository>();
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


            // Загружаем настройки JWT
            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var signingKey = jwtSettings["SigningKey"];

            if (string.IsNullOrEmpty(signingKey))
            {
                throw new Exception("JWT Signing Key is missing from configuration!");
            }

            // Декодируем ключ из Base64
            var keyBytes = Convert.FromBase64String(signingKey);

            if (keyBytes.Length < 32)
            {
                throw new Exception($"JWT Key must be at least 256 bits (32 bytes), but got {keyBytes.Length} bytes.");
            }

            Console.WriteLine($"Decoded JWT Key bytes length: {keyBytes.Length} bytes");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audiences:0"],
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                        ClockSkew = TimeSpan.Zero // Уменьшите погрешность времени
                    };

                  
                });



            builder.Services.AddAuthorization();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePost).Assembly));

        }

        public static void RegisterEndpointDefinitions(this WebApplication app)
        {
            var endpointDefinitions = typeof(Program).Assembly.GetTypes()
                .Where(t=>t.IsAssignableTo(typeof(IEndpointDefinition))&& !t.IsAbstract && !t.IsInterface)
                .Select(Activator.CreateInstance)
                .Cast<IEndpointDefinition>();

            foreach(var endpointDefinition in endpointDefinitions)
            {
                endpointDefinition.RegisterEndpoints(app);
            }
        }
    }
}
