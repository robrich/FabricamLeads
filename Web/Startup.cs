using Fabricam.Web.Core.DataAccess;
using Fabricam.Web.Core.Models;
using Fabricam.Web.Core.Repositories;
using Fabricam.Web.Core.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricam.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AppSettings settings = new AppSettings();
            this.Configuration.Bind(settings);
            services.AddSingleton(settings);

            services.AddDbContext<FabricamDbContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ILeadRepository, LeadRepository>();
            services.AddScoped<IRandomLeadRepository, RandomLeadRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ILoadLeadsService, LoadLeadsService>();

            // Using cookie auth without ASP.NET Identity: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/cookie?tabs=aspnetcore2x
            // See also: https://github.com/aspnet/Docs/tree/master/aspnetcore/security/authentication/cookie/sample
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                // TODO: add Serilog and pipe errors to corporate logging store
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();
        }

    }
}
