using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
    }
}
