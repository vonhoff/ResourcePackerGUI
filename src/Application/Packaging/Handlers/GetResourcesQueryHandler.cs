using MediatR;
using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Application.Packaging.Queries;
using ResourcePackerGUI.Domain.Entities;
using ResourcePackerGUI.Domain.Structures;

namespace ResourcePackerGUI.Application.Packaging.Handlers
{
    public class GetResourcesQueryHandler : IRequestHandler<GetResourcesQuery, IReadOnlyList<Resource>>
    {
        private readonly IAesEncryptionService _aesEncryptionService;
        private readonly ICrc32Service _crc32Service;
        private readonly ILogger<GetResourcesQueryHandler> _logger;
        private readonly IMediaTypeService _mediaTypeService;

        public GetResourcesQueryHandler(IAesEncryptionService aesEncryptionService,
            ICrc32Service crc32Service,
            IMediaTypeService mediaTypeService,
            ILogger<GetResourcesQueryHandler> logger)
        {
            _aesEncryptionService = aesEncryptionService;
            _crc32Service = crc32Service;
            _mediaTypeService = mediaTypeService;
            _logger = logger;
        }

        public Task<IReadOnlyList<Resource>> Handle(GetResourcesQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var assets = GetResources(request, cancellationToken);
                if (assets.Count == request.Entries.Count)
                {
                    _logger.LogInformation("Loaded all {assetCount} assets.", assets.Count);
                }
                else
                {
                    _logger.LogWarning("Loaded {assetCount} out of {entryCount} assets.",
                        assets.Count, request.Entries.Count);
                }

                return assets;
            }, cancellationToken);
        }

        private IReadOnlyList<Resource> GetResources(GetResourcesQuery request, CancellationToken cancellationToken)
        {
            using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);
            var assets = new List<Resource>();
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
                    continue;
                }

                var mimeType = _mediaTypeService.GetTypeByData(buffer);
                var asset = new Resource(buffer[..entry.DataSize], entry, mimeType);
                assets.Add(asset);
                percentage = (int)((double)(i + 1) / request.Entries.Count * 100);
                _logger.LogDebug("Added asset: {@asset}",
                    new { asset.Name, asset.MediaType });
            }

            return assets;
        }

        private bool TryLoadResourceData(GetResourcesQuery request, Entry entry, out byte[] buffer,
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

            if (!_aesEncryptionService.DecryptCbc(buffer, out var output, key,
                    request.ProgressSecondary, request.ProgressReportInterval, cancellationToken))
            {
                return false;
            }

            buffer = output;
            return true;
        }
    }
}