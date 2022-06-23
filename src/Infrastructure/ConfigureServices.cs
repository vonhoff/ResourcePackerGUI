using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Infrastructure.Services;

namespace ResourcePackerGUI.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<IAesEncryptionService, AesEncryptionService>();
            services.AddSingleton<ICrc32Service, Crc32Service>();
            services.AddSingleton<IMediaTypeService, MediaTypeService>();
            services.AddSingleton<IFileSystem, FileSystem>();
            return services;
        }
    }
}