using ApplicationAPI.Data;
using ApplicationAPI.Helpers;
using ApplicationAPI.Interfaces;
using ApplicationAPI.Services;
using ApplicationAPI.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ApplicationAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSignalR();
            services.AddSingleton<PresenceTracker>();
            
            return services;
        }
    }
}