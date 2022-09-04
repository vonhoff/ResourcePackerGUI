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

using MediatR;
using ResourcePackerGUI.Application.Common.Exceptions;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Domain.Entities;
using ResourcePackerGUI.Domain.Structures;
using Serilog;

namespace ResourcePackerGUI.Application.Packaging.Queries.GetPackageResources
{
    public class GetPackageResourcesQueryHandler : IRequestHandler<GetPackageResourcesQuery, IReadOnlyList<Resource>>
    {
        private readonly IAesEncryptionService _aesEncryptionService;
        private readonly ICrc32Service _crc32Service;
        private readonly IMediaTypeService _mediaTypeService;

        public GetPackageResourcesQueryHandler(IAesEncryptionService aesEncryptionService,
            ICrc32Service crc32Service,
            IMediaTypeService mediaTypeService)
        {
            _aesEncryptionService = aesEncryptionService;
            _crc32Service = crc32Service;
            _mediaTypeService = mediaTypeService;
        }

        public Task<IReadOnlyList<Resource>> Handle(GetPackageResourcesQuery request, CancellationToken cancellationToken)
        {
            var resources = GetResources(request, cancellationToken);
            if (resources.Count == request.Entries.Count)
            {
                Log.Information("Loaded all {assetCount} resources.", resources.Count);
            }
            else
            {
                Log.Warning("Loaded {assetCount} out of {entryCount} resources.",
                    resources.Count, request.Entries.Count);
            }

            request.ProgressPrimary?.Report(100f);
            return Task.FromResult(resources);
        }

        private IReadOnlyList<Resource> GetResources(GetPackageResourcesQuery request, CancellationToken cancellationToken)
        {
            using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);
            var resources = new List<Resource>();
            var percentage = 0f;
            var key = string.IsNullOrEmpty(request.Password)
                ? Array.Empty<uint>()
                : _aesEncryptionService.KeySetup(request.Password);

            // ReSharper disable once AccessToModifiedClosure
            progressTimer.Elapsed += delegate { request.ProgressPrimary!.Report(percentage); };
            progressTimer.Enabled = request.ProgressPrimary != null;

            for (var i = 0; i < request.Entries.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var entry = request.Entries[i];

                if (!TryLoadResourceData(request, entry, out var buffer, key, cancellationToken))
                {
                    continue;
                }

                var crc = _crc32Service.Compute(buffer, 0, entry.DataSize);
                if (entry.Crc != crc)
                {
                    if (key.Length > 0)
                    {
                        throw new InvalidPasswordException("The password entered is incorrect.", request.Password.GetHashCode());
                    }

                    Log.Warning("Integrity check failed: {@results}", new { entry.Id, Expected = entry.Crc, Actual = crc });
                    continue;
                }

                var mimeType = _mediaTypeService.GetTypeByData(buffer);
                var resource = new Resource(buffer, entry, mimeType);
                resources.Add(resource);
                percentage = (i + 1f) / request.Entries.Count * 100f;
                Log.Debug("Added resource: {@asset}", new { resource.Name, MediaType = mimeType?.Name });
            }

            return resources;
        }

        private bool TryLoadResourceData(GetPackageResourcesQuery request, Entry entry, out byte[] buffer,
            uint[] key, CancellationToken cancellationToken)
        {
            try
            {
                request.BinaryReader.BaseStream.Position = entry.Offset;
                buffer = request.BinaryReader.ReadBytes(entry.PackSize);
            }
            catch
            {
                buffer = Array.Empty<byte>();
                return false;
            }

            if (key.Length == 0)
            {
                return true;
            }

            if (!_aesEncryptionService.DecryptCbc(buffer, entry.DataSize, out var output, key,
                    request.ProgressSecondary, request.ProgressReportInterval, cancellationToken))
            {
                return false;
            }

            buffer = output;
            return true;
        }
    }
}