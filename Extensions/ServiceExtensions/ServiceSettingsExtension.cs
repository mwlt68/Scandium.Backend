using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scandium.Model.Settings;

namespace Scandium.Extensions.ServiceExtensions
{
    public static class ServiceSettingsExtension
    {
        public static void AddSettings (this IServiceCollection services,IConfiguration configuration)
        {
            var jwtSettingsSection = 
                configuration.GetSection("Jwt");
            services.Configure<JwtSettings>(jwtSettingsSection);
        }
    }
}