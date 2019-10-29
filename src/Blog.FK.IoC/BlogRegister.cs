using Blog.FK.Application;
using Blog.FK.Application.Interfaces;
using Blog.FK.Domain.Interfaces;
using Blog.FK.Infra.DataContext;
using Blog.FK.Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.FK.IoC
{
    public class BlogRegister
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<DbContext, BlogContext>();
            services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            services.AddScoped<IIORepository, IORepository>();
            services.AddScoped<IBlogPostApplication, BlogPostApplication>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserApplication, UserApplication>();
            services.AddTransient<IPushSubscriptionApplication, PushSubscriptionApplication>();
        }
    }
}
