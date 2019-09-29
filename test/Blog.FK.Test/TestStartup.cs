using AutoMapper;
using Blog.FK.Infra.DataContext;
using Blog.FK.IoC;
using Blog.FK.Web.Extensions;
using Blog.FK.Web.Profiles;
using Blog.FK.Web.Validators;
using FluentValidation.AspNetCore;
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

namespace Blog.FK.Test
{
    public class TestStartup
    {
        public IConfiguration Configuration { get; }

        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddDefaultJsonOptions()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<BlogPostViewModelValidator>());

            services.AddAutoMapper(typeof(BlogPostProfile), typeof(UserProfile));

            services.AddEntityFrameworkSqlite().AddDbContext<BlogContext>(options =>
            {
                options.UseSqlite(Configuration["BlogFKConn:ConnectionString"],
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

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();

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
