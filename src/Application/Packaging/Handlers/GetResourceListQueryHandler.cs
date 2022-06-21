using MediatR;
using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Application.Packaging.Queries;
using ResourcePackerGUI.Domain.Entities;
using ResourcePackerGUI.Domain.ValueObjects;

namespace ResourcePackerGUI.Application.Packaging.Handlers
{
    public class GetResourceListQueryHandler : IRequestHandler<GetResourceListQuery, IReadOnlyList<Resource>>
    {
        private readonly IAesEncryptionService _aesEncryptionService;
        private readonly ICrc32Service _crc32Service;
        private readonly ILogger<GetResourceListQueryHandler> _logger;
        private readonly IMimeTypeService _mimeTypeService;

        public GetResourceListQueryHandler(IAesEncryptionService aesEncryptionService,
            ICrc32Service crc32Service,
            IMimeTypeService mimeTypeService,
            ILogger<GetResourceListQueryHandler> logger)
        {
            _aesEncryptionService = aesEncryptionService;
            _crc32Service = crc32Service;
            _mimeTypeService = mimeTypeService;
            _logger = logger;
        }

        public Task<IReadOnlyList<Resource>> Handle(GetResourceListQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var assets = GetAssetList(request, cancellationToken);
                var entryCount = request.Package.Entries.Count;

                if (assets.Count == entryCount)
                {
                    _logger.LogInformation("Loaded all {assetCount} assets.", assets.Count);
                }
                else
                {
                    _logger.LogWarning("Loaded {assetCount} out of {entryCount} assets.",
                        assets.Count, entryCount);
                }

                return assets;
            }, cancellationToken);
        }

        private IReadOnlyList<Resource> GetAssetList(GetResourceListQuery request, CancellationToken cancellationToken)
        {
            using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);
            var assets = new List<Resource>();
            var percentage = 0;
            var entries = request.Package.Entries;

            // ReSharper disable once AccessToModifiedClosure
            progressTimer.Elapsed += delegate { request.ProgressPrimary!.Report(percentage); };
            progressTimer.Enabled = request.ProgressPrimary != null;

            for (var i = 0; i < entries.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var entry = entries[i];

                request.BinaryReader.BaseStream.Position = entry.Offset;
                if (!TryLoadAsset(request, entry, out var buffer, cancellationToken))
                {
                    continue;
                }

                var crc = _crc32Service.Compute(buffer, 0, entry.DataSize);
                if (entry.Crc != crc)
                {
                    continue;
                }

                var mimeType = _mimeTypeService.GetTypeByData(buffer);
                var asset = new Resource(buffer, entry, mimeType);
                assets.Add(asset);
                percentage = (int)((double)(i + 1) / entries.Count * 100);
                _logger.LogDebug("Added asset: {@asset}", 
                    new { asset.Name, asset.MediaType });
            }

            return assets;
        }

        private bool TryLoadAsset(GetResourceListQuery request, Entry entry, out byte[] buffer, 
            CancellationToken cancellationToken)
        {
            try
            {
                buffer = request.BinaryReader.ReadBytes(entry.PackSize);
            }
            catch
            {
                buffer = Array.Empty<byte>();
                return false;
            }
            
            if (string.IsNullOrEmpty(request.Password))
            {
                return true;
            }

            var key = _aesEncryptionService.KeySetup(request.Password);
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