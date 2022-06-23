using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ResourcePackerGUI.Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}