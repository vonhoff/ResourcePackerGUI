#region GNU General Public License

/* Copyright 2022 Simon Vonhoff & Contributors
 *
 * This file is part of ResourcePackerGUI.
 *
 * ResourcePackerGUI is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 *
 * ResourcePackerGUI is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with ResourcePackerGUI.
 * If not, see <https://www.gnu.org/licenses/>.
 */

#endregion

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