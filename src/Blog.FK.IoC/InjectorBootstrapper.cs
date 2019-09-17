using Microsoft.Extensions.DependencyInjection;

namespace Blog.FK.IoC
{
    public class InjectorBootstrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            BlogRegister.Register(services);
        }
    }
}
