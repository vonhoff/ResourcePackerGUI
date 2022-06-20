using MediatR;
using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.Common.Exceptions;
using ResourcePackerGUI.Application.Common.Extensions;
using ResourcePackerGUI.Application.Packaging.Queries;
using ResourcePackerGUI.Domain.Entities;
using ResourcePackerGUI.Domain.ValueObjects;

namespace ResourcePackerGUI.Application.Packaging.Handlers
{
    public class GetPackageInformationQueryHandler : IRequestHandler<GetPackageInformationQuery, Package>
    {
        private const ulong PackHeaderId = 0x6B636150736552;
        private readonly ILogger<GetPackageInformationQueryHandler> _logger;

        public GetPackageInformationQueryHandler(ILogger<GetPackageInformationQueryHandler> logger)
        {
            _logger = logger;
        }

        public Task<Package> Handle(GetPackageInformationQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var header = GetHeader(request.BinaryReader);
                var entries = GetEntries(request, header, cancellationToken);

                if (entries.Count == header.NumberOfEntries)
                {
                    _logger.LogInformation("Loaded all {entryCount} entries.", entries.Count);
                }
                else
                {
                    _logger.LogWarning("Loaded {entryCount} out of {expectedCount} entries.",
                        entries.Count, header.NumberOfEntries);
                }

                return new Package(header, entries);
            }, cancellationToken);
        }

        private static PackageHeader GetHeader(BinaryReader binaryReader)
        {
            var header = binaryReader.ReadStruct<PackageHeader>();
            if (header.Id != PackHeaderId || header.NumberOfEntries <= 0)
            {
                throw new InvalidHeaderException("The header of the provided stream is invalid.");
            }

            return header;
        }

        private List<Entry> GetEntries(GetPackageInformationQuery request, PackageHeader header, CancellationToken cancellationToken)
        {
            var entries = new List<Entry>();
            using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);
            var percentage = 0;
            progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
            progressTimer.Enabled = request.Progress != null;

            for (var i = 0; i < header.NumberOfEntries; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (!GetEntry(request, out var entry))
                {
                    continue;
                }

                entries.Add(entry);
                _logger.LogDebug("Added entry: {@entry}", new { entry.Id, entry.Crc, entry.DataSize });
                percentage = (int)((double)(i + 1) / header.NumberOfEntries * 100);
            }

            return entries;
        }

        private bool GetEntry(GetPackageInformationQuery request, out Entry entry)
        {
            entry = request.BinaryReader.ReadStruct<Entry>();
            if (entry.Id != 0)
            {
                return true;
            }

            _logger.LogWarning("Invalid entry: {@entry}", new { entry.Id, entry.Crc, entry.DataSize, entry.PackSize });
            return false;
        }
    }
}