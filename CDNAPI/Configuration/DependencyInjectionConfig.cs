using CDNAPI.Interfaces;
using CDNAPI.Models;
using CDNAPI.Models.Validations;
using CDNAPI.Repository;
using CDNAPI.Services;
using CDNAPI.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<ApiDbContext>();
            services.AddScoped<IEntityLogRepository, EntityLogRepository>();
            services.AddScoped<IEntityLogService, EntityLogService>();

            services.AddScoped<IFileUtilsService,FileUtilsService>();

            return services;
        }
    }
}
