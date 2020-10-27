using AIG.Services.InstallationToken;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AIG.Controllers;
using AIG.Constants;

namespace AIG
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
            services.AddTransient<IInstallationTokenService, InstallationTokenService>();
            SetupJWTAuth(services);
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void SetupJWTAuth(IServiceCollection services)
        {
            #region Authentication
            services.AddAuthentication(o => {
                o.DefaultScheme = AigAuthConstants.TokenAuthenticationDefaultScheme;
            })
            .AddScheme<JwtBearerOptions, TokenAuthenticationHandler>(AigAuthConstants.TokenAuthenticationDefaultScheme, o => { });
            #endregion
        }
    }
}
