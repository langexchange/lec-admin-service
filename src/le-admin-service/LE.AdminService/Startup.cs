using AutoMapper;
using LE.AdminService.AutoMappers;
using LE.AdminService.Extensions;
using LE.AdminService.Infrastructure.Infrastructure;
using LE.AdminService.Services;
using LE.AdminService.Services.Implements;
using LE.Library.Consul;
using LE.Library.Host;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LE.AdminService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LE.AdminService", Version = "v1" });
            });

            services.AddHttpContextAccessor();
            services.AddCustomAuthorization(Configuration);
            services.AddConsul();
            services.AddRequestHeader();
            services.AddScoped<ISettingService, SettingService>();
            services.AddSingleton<IAuthorizationHandler, AuthRequirementHandler>();
            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, Services.Implements.UserService>();

            AddAutoMappers(services);
            AddDbContext(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LE.AdminService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseConsul();

            app.UseCors(x => x
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(origin => true) // allow any origin
              .AllowCredentials()); // allow credentials

            app.UseCustomAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddAutoMappers(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new SettingProfile());
                mc.AddProfile(new AdminProfile());
                mc.AddProfile(new UserProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
        private void AddDbContext(IServiceCollection services)
        {
            services.AddDbContext<LanggeneralDbContext>(options => options.UseNpgsql(Env.DB_CONNECTION_STRING));
        }
    }
}
