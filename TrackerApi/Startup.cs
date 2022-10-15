using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using TrackerApi.Data;
using TrackerApi.Services.ActorService;
using TrackerApi.Services.EpisodeService;
using TrackerApi.Services.JwtService;
using TrackerApi.Services.LoginService;
using TrackerApi.Services.SharedServices;
using TrackerApi.Services.TvShowService;
using TrackerApi.Services.UserService;
using TrackerApi.Transaction;

namespace TrackerApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddDbContext<AppDbContext>();
            services.AddScoped<AppDbContext, AppDbContext>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITvShowService, TvShowService>();
            services.AddScoped<IEpisodeService, EpisodeService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IActorService, ActorService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<DbContextService, DbContextService>();

            services.AddControllers();

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(token =>
            {
                token.RequireHttpsMetadata = false;
                token.SaveToken = true;
                token.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddHangfire(configuration => configuration.UseMemoryStorage());

            services.AddHangfireServer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();


            app.UseHangfireDashboard();

            BackgroundJob.Enqueue(() => LoadTvShows());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public async Task LoadTvShows()
        {
            await Task.Run(async () =>
            {
                var client = new HttpClient();
                var stringContent = new StringContent("");


                await client.PostAsync("https://localhost:5001/v1/tvshows/load", stringContent);
            });
        }
    }
}
