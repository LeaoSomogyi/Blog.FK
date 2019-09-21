using AutoMapper;
using Blog.FK.Infra.DataContext;
using Blog.FK.IoC;
using Blog.FK.Web.Extensions;
using Blog.FK.Web.Profiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Blog.FK.Web
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
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddDefaultJsonOptions();

            services.AddAutoMapper(typeof(BlogPostProfile), typeof(UserProfile));

            services.AddEntityFrameworkSqlServer().AddDbContext<BlogContext>(options =>
            {
                options.UseSqlServer(Configuration["BlogFKConn:ConnectionString"],
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(BlogContext)
                    .GetTypeInfo().Assembly.GetName().Name));
            });

            #region "  Register Services  "

            InjectorBootstrapper.RegisterServices(services);

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

            #endregion

            #region "  Configure Auth  "

            ConfigureAuth(services);

            #endregion

            services.AddSession();
        }

        private void ConfigureAuth(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Blog/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Blog}/{action=Index}/{id?}");
            });

            //Migrate database if needed
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<DbContext>();
                context.Database.Migrate();
            }
        }
    }
}
