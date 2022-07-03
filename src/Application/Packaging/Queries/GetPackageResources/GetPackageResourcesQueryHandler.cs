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
            var assets = GetResources(request, cancellationToken);
            if (assets.Count == request.Entries.Count)
            {
                Log.Information("Loaded all {assetCount} assets.", assets.Count);
            }
            else
            {
                Log.Warning("Loaded {assetCount} out of {entryCount} assets.",
                    assets.Count, request.Entries.Count);
            }

            request.ProgressPrimary?.Report(100);
            return Task.FromResult(assets);
        }

        private IReadOnlyList<Resource> GetResources(GetPackageResourcesQuery request, CancellationToken cancellationToken)
        {
            using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);
            var resources = new List<Resource>();
            var percentage = 0;
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

                    continue;
                }

                var mimeType = _mediaTypeService.GetTypeByData(buffer);
                var resource = new Resource(buffer, entry, mimeType);
                resources.Add(resource);
                percentage = (int)((double)(i + 1) / request.Entries.Count * 100);
                Log.Debug("Added asset: {@asset}",
                    new { resource.Name, MediaType = mimeType?.Name });
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