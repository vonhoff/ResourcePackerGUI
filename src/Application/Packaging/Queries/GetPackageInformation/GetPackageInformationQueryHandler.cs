using MediatR;
using ResourcePackerGUI.Application.Common.Exceptions;
using ResourcePackerGUI.Application.Common.Extensions;
using ResourcePackerGUI.Domain.Entities;
using ResourcePackerGUI.Domain.Structures;
using Serilog;

namespace ResourcePackerGUI.Application.Packaging.Queries.GetPackageInformation
{
    public class GetPackageInformationQueryHandler : IRequestHandler<GetPackageInformationQuery, Package>
    {
        private const ulong PackHeaderId = 30227092120757586;

        public Task<Package> Handle(GetPackageInformationQuery request, CancellationToken cancellationToken)
        {
            var header = GetHeader(request.BinaryReader);
            var entries = GetEntries(request, header, cancellationToken);

            if (entries.Count == header.NumberOfEntries)
            {
                Log.Information("Loaded all {entryCount} entries.", entries.Count);
            }
            else
            {
                Log.Warning("Loaded {entryCount} out of {expectedCount} entries.",
                    entries.Count, header.NumberOfEntries);
            }

            var encrypted = entries.Any(e => e.DataSize != e.PackSize);
            var package = new Package(header, entries, encrypted);

            Log.Information("ResourcePackage: {@info}",
                new { package.Header.Id, package.Header.NumberOfEntries, package.Encrypted });

            return Task.FromResult(package);
        }

        /// <summary>
        /// Retrieves and validates the header of the provided <see cref="BinaryReader"/>.
        /// </summary>
        /// <param name="binaryReader">The binary reader of the specified file stream.</param>
        /// <returns>A <see cref="PackageHeader"/> instance containing information about the header.</returns>
        /// <exception cref="InvalidHeaderException">When the header of the provided stream is invalid.</exception>
        private static PackageHeader GetHeader(BinaryReader binaryReader)
        {
            var header = binaryReader.ReadStruct<PackageHeader>();
            if (header.Id != PackHeaderId || header.NumberOfEntries <= 0)
            {
                throw new InvalidHeaderException("The header of the provided stream is invalid.", header.Id);
            }

            return header;
        }

        /// <summary>
        /// Retrieves all entries from the package binary reader.
        /// </summary>
        /// <param name="request">The request containing the binary reader and progress instances.</param>
        /// <param name="header">The package header containing all package information.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
        /// <returns>A read-only list of entries.</returns>
        private static IReadOnlyList<Entry> GetEntries(GetPackageInformationQuery request, PackageHeader header, CancellationToken cancellationToken)
        {
            using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);

            var percentage = 0;
            // ReSharper disable once AccessToModifiedClosure
            progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
            progressTimer.Enabled = request.Progress != null;

            var entries = new List<Entry>();
            for (var i = 0; i < header.NumberOfEntries; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (!GetEntry(request.BinaryReader, out var entry))
                {
                    continue;
                }

                entries.Add(entry);
                Log.Debug("Added entry: {@entry}", new { entry.Id, entry.Crc, entry.DataSize, entry.PackSize });
                percentage = (int)((double)(i + 1) / header.NumberOfEntries * 100);
            }

            request.Progress?.Report(100);
            return entries;
        }

        /// <summary>
        /// Reads the next entry from the binary stream.
        /// </summary>
        /// <param name="binaryReader">The binary reader of the specified file stream.</param>
        /// <param name="entry">The resulting entry.</param>
        /// <returns><see langword="true"/> when successful, <see langword="false"/> otherwise.</returns>
        private static bool GetEntry(BinaryReader binaryReader, out Entry entry)
        {
            entry = binaryReader.ReadStruct<Entry>();
            if (entry.Id != 0)
            {
                return true;
            }

            Log.Warning("Invalid entry: {@entry}", new { entry.Id, entry.Crc, entry.DataSize, entry.PackSize });
            return false;
        }
    }
}