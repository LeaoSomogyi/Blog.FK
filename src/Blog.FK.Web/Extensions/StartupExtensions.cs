using AutoMapper;
using Blog.FK.Infra.DataContext;
using Blog.FK.Web.Profiles;
using Lib.Net.Http.WebPush;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Blog.FK.Web.Extensions
{
    public static class StartupExtensions
    {
        /// <summary>
        /// Method to add general JSON configurations
        /// </summary>
        /// <param name="builder">this IMvcBuilder</param>
        /// <returns>IMvcBuilder configured</returns>
        public static IMvcBuilder AddDefaultJsonOptions(this IMvcBuilder builder)
        {
            return builder.AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                };

                opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                opt.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
                opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                opt.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss";
                opt.SerializerSettings.Culture = new System.Globalization.CultureInfo("en-US");
                opt.SerializerSettings.Formatting = Formatting.None;
                opt.SerializerSettings.FloatFormatHandling = FloatFormatHandling.DefaultValue;
                opt.SerializerSettings.FloatParseHandling = FloatParseHandling.Double;
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.TypeNameHandling = TypeNameHandling.None;
            });
        }

        /// <summary>
        /// Method to add AutoMapper profiles
        /// </summary>
        /// <param name="services">this IServiceCollection</param>
        /// <returns>IServiceCollection with AutoMapper profiles</returns>
        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services) 
        {
            services.AddAutoMapper(typeof(BlogPostProfile), 
                typeof(UserProfile), 
                typeof(PushSubscriptionProfile),
                typeof(PushMessageProfile));

            return services;
        }

        /// <summary>
        /// Method to configure EntityFramework SQL Server
        /// </summary>
        /// <param name="services">this IServiceCollection</param>
        /// <param name="configuration">IConfiguration</param>
        /// <returns>IServiceCollection with SQL Server options</returns>
        public static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer().AddDbContext<BlogContext>(options =>
            {
                options.UseSqlServer(configuration["BlogFKConn:ConnectionString"],
                    sqlOptions => sqlOptions.MigrationsAssembly(typeof(BlogContext)
                    .GetTypeInfo().Assembly.GetName().Name));
            });

            return services;
        }

        /// <summary>
        /// Method to configure PushSubscription services and SQLite
        /// </summary>
        /// <param name="services">this IServiceCollection</param>
        /// <param name="configuration">IConfiguration</param>
        /// <returns>IServiceCollection with PushSubscription configured</returns>
        public static IServiceCollection AddPushSubscriptionService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PushSubscriptionContext>(options =>
            {
                options.UseSqlite(configuration["BlogFKConn:SqliteConnectionString"]);
            });

            services.AddOptions();
            services.AddMemoryCache();
            services.AddMemoryVapidTokenCache();
            services.AddPushServiceClient(options =>
            {
                var pushNotificationServiceConfigurationSection = configuration.GetSection(nameof(PushServiceClient));

                options.Subject = pushNotificationServiceConfigurationSection.GetValue<string>(nameof(options.Subject));
                options.PublicKey = pushNotificationServiceConfigurationSection.GetValue<string>(nameof(options.PublicKey));
                options.PrivateKey = pushNotificationServiceConfigurationSection.GetValue<string>(nameof(options.PrivateKey));
            });


            return services;
        }
    }
}
