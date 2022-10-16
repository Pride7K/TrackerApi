using Microsoft.Extensions.DependencyInjection;
using TrackerApi.Data;
using TrackerApi.Services.ActorService;
using TrackerApi.Services.EpisodeService;
using TrackerApi.Services.LoginService;
using TrackerApi.Services.SharedServices;
using TrackerApi.Services.TvShowService;
using TrackerApi.Services.UserService;
using TrackerApi.Transaction;

namespace TrackerApi
{
    public static class DependencyInjectionRegistry
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            services.AddScoped<AppDbContext, AppDbContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITvShowService, TvShowService>();
            services.AddScoped<IEpisodeService, EpisodeService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IActorService, ActorService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DbContextService, DbContextService>();

            return services;
        }
    }
}
