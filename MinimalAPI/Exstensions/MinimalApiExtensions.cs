using Application.Abstractions;
using Application.Posts.Commands;
using DataAccess.Repositories;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using MinimalAPI.Abstractions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccess.Services;
using Application.Notifications.Decorators;
using Google.Api.Gax.Rest;
using Application.Images;
using DataAccess.Storage;
using DataAccess.Proxies;
using ILogger = Application.Abstractions.ILogger;
using Application.Export;

namespace MinimalAPI.Exstensions
{
    public static class MinimalApiExtensions
    {

        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            var cs = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<BlogDbContext>(opt => opt.UseSqlServer(cs));

            builder.Services.AddSingleton<ILogger, Logger>();

            builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<ArticleRepository>();

            builder.Services.AddScoped<IArticleRepository>(provider => {
                var articleRepo = provider.GetRequiredService<ArticleRepository>();
                var logger = provider.GetRequiredService<ILogger>();
                return new ArticleRepositoryProxy(articleRepo, logger);
            });
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<ILikeRepository, LikeRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            // В Program.cs или метод RegisterServices
            builder.Services.AddImageStorage(builder.Configuration);
            builder.Services.AddScoped<IImageManager, ArticleImageManager>();

            builder.Services.AddScoped<ArticleExporterFactory>();
            builder.Services.AddScoped<ArticleExportService>();

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            builder.Services.AddHttpClient("SmsService");
            //builder.Services.AddScoped<IImageStorage>(provider =>
            //{
            //    var config = provider.GetRequiredService<IConfiguration>();

            //    var basePath = config["ImageStorage:Local:BasePath"];
            //    var baseUrl = config["ImageStorage:Local:BaseUrl"];

            //    return new LocalFileStorage(basePath, baseUrl);
            //});

            // Регистрация репозитория для телефонов
            builder.Services.AddScoped<IUserPhoneRepository, UserPhoneRepository>();

            // Регистрация отправителей уведомлений
            builder.Services.AddScoped<INotificationSender>(provider => {
                var notificationRepository = provider.GetRequiredService<INotificationRepository>();

                // Базовый отправщик, который сохраняет в БД
                INotificationSender sender = new DatabaseNotificationSender(notificationRepository);

                // Декорируем отправкой по Email
                sender = new EmailNotificationSender(
                    sender,
                    provider.GetRequiredService<IAuthRepository>(),
                    provider.GetRequiredService<IConfiguration>(),
                    provider.GetRequiredService<ILogger<EmailNotificationSender>>());

                // Декорируем отправкой по SMS
                sender = new SmsNotificationSender(
                    sender,
                    provider.GetRequiredService<IConfiguration>(),
                    provider.GetRequiredService<IHttpClientFactory>(),
                    provider.GetRequiredService<IUserPhoneRepository>(),
                    provider.GetRequiredService<ILogger<SmsNotificationSender>>());

                return sender;
            });


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
            builder.Services.AddHostedService<CacheCleanupService>();

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
