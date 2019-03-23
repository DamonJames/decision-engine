using DecisionEngine.Models;
using DecisionEngine.Repositories.Implementation;
using DecisionEngine.Repositories.Interfaces;
using DecisionEngine.Services.Implementation;
using DecisionEngine.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DecisionEngine.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddOptions();

            // Singletons
            services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));

            services.AddSingleton(sp => sp.GetRequiredService<IOptions<ApiSettings>>().Value);
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<ConnectionStrings>>().Value);

            // Bindings
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IHttpRequester, HttpRequester>();
            services.AddTransient<IConnectionFactory, ConnectionFactory>();

        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

#pragma warning disable CS0618 // Type or member is obsolete
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
#pragma warning restore CS0618 // Type or member is obsolete

            app.UseStatusCodePages("text/plain", "Status code page, status code: {0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Input}/{action=Index}/{id?}");
            });
        }
    }
}
