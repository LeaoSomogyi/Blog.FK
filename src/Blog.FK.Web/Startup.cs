using AutoMapper;
using Blog.FK.Domain.Configurations;
using Blog.FK.Infra.DataContext;
using Blog.FK.IoC;
using Blog.FK.Web.Extensions;
using Blog.FK.Web.Filters;
using Blog.FK.Web.Profiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
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

            var tokenConfig = new TokenConfigurations();
            Configuration.Bind("TokenConfigurations", tokenConfig);

            var signingConfigurations = new SigningConfigurations();

            services.AddSingleton(tokenConfig);
            services.AddSingleton(signingConfigurations);

            services.AddScoped(typeof(AuthorizationFilter));

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.AddService<AuthorizationFilter>(1);
            });

            #endregion

            #region "  Configure Auth  "

            ConfigureAuth(services, signingConfigurations);

            #endregion
        }

        private void ConfigureAuth(IServiceCollection services, SigningConfigurations signingConfigurations)
        {
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(bearerOptions =>
            {
                bearerOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = signingConfigurations.SecurityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

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
