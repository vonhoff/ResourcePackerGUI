﻿using System.IO.Abstractions;
using System.Runtime.CompilerServices;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;
using ResourcePackerGUI.Application.Common.Extensions;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Application.Packaging.Queries;
using ResourcePackerGUI.Domain.Structures;

namespace ResourcePackerGUI.Application.Packaging.Handlers
{
    public class BuildPackageQueryHandler : IRequestHandler<BuildPackageQuery>
    {
        private const ulong PackHeaderId = 30227092120757586;
        private readonly IAesEncryptionService _aesEncryptionService;
        private readonly ICrc32Service _crc32Service;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<BuildPackageQueryHandler> _logger;

        public BuildPackageQueryHandler(IFileSystem fileSystem,
            IAesEncryptionService aesEncryptionService,
            ICrc32Service crc32Service,
            ILogger<BuildPackageQueryHandler> logger)
        {
            _fileSystem = fileSystem;
            _aesEncryptionService = aesEncryptionService;
            _crc32Service = crc32Service;
            _logger = logger;
        }

        public Task<Unit> Handle(BuildPackageQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var header = new PackageHeader
                {
                    Id = PackHeaderId,
                    NumberOfEntries = request.PathEntries.Count
                };

                using (var outputStream = _fileSystem.File.OpenWrite(request.Output))
                {
                    using var binaryWriter = new BinaryWriter(outputStream);
                    var initialOffset = Unsafe.SizeOf<PackageHeader>() + (header.NumberOfEntries * Unsafe.SizeOf<Entry>());
                    var key = _aesEncryptionService.KeySetup(request.Password);

                    // Write all assets to file and retrieve their associated entry data.
                    var entries = WriteAssets(request, initialOffset, key, binaryWriter, cancellationToken);

                    // Write header to file.
                    binaryWriter.Seek(0, SeekOrigin.Begin);
                    binaryWriter.WriteStruct(header);

                    // Write entries to file.
                    foreach (var entry in entries)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        binaryWriter.WriteStruct(entry);
                    }
                }

                return Unit.Value;
            }, cancellationToken);
        }

        /// <summary>
        /// Creates a new entry from the provided file content and relative path.
        /// </summary>
        /// <param name="offset">The offset to include in the entry.</param>
        /// <param name="fileContent">The file contents to be used to calculate a CRC32 value.</param>
        /// <param name="relativePath">The relative path to be used to calculate a CRC32 value.</param>
        /// <returns>An <see cref="Entry"/> instance.</returns>
        private Entry CreateEntry(int offset, byte[] fileContent, string relativePath)
        {
            var dataSize = fileContent.Length;
            var nameBytes = Encoding.ASCII.GetBytes(relativePath);
            var nameCrc = _crc32Service.Compute(nameBytes);
            var fileCrc = _crc32Service.Compute(fileContent);

            return new Entry
            {
                Id = nameCrc,
                Crc = fileCrc,
                Offset = offset,
                DataSize = dataSize,
                PackSize = dataSize
            };
        }

        /// <summary>
        /// Attempts to retrieve the file contents of the specified path.
        /// </summary>
        /// <param name="absolutePath">The location to read the file contents from.</param>
        /// <param name="packageOutput">The output of the package.</param>
        /// <param name="fileContent">The retrieved file contents from <paramref name="absolutePath"/>.</param>
        /// <returns><see langword="true"/> when succeeded, <see langword="false"/> when an exception occurred.</returns>
        /// <exception cref="InvalidOperationException">
        /// When <paramref name="absolutePath"/> is the same as <paramref name="packageOutput"/>.</exception>
        private bool TryGetFileContents(string absolutePath, string packageOutput, out byte[] fileContent)
        {
            try
            {
                if (absolutePath.Equals(packageOutput))
                {
                    throw new InvalidOperationException("The specified file is the same as the output file.");
                }

                fileContent = _fileSystem.File.ReadAllBytes(absolutePath);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not read file from: {path}", absolutePath);
                fileContent = Array.Empty<byte>();
                return false;
            }
        }

        /// <summary>
        /// Writes assets from the provided settings in <see cref="BuildPackageQuery"/>.
        /// </summary>
        /// <param name="request">The request containing the entries and progress instances.</param>
        /// <param name="offset">The initial offset for writing the assets.</param>
        /// <param name="key"></param>
        /// <param name="binaryWriter">The writer is responsible for writing the assets.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the process if necessary.</param>
        /// <returns>The entries created for the assets.</returns>
        private IEnumerable<Entry> WriteAssets(BuildPackageQuery request, int offset, uint[] key,
            BinaryWriter binaryWriter, CancellationToken cancellationToken)
        {
            var entries = new Entry[request.PathEntries.Count];
            var percentage = 0;

            using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);

            // ReSharper disable once AccessToModifiedClosure
            progressTimer.Elapsed += delegate { request.ProgressPrimary!.Report(percentage); };
            progressTimer.Enabled = request.ProgressPrimary != null;

            for (var i = 0; i < request.PathEntries.Count; i++)
            {
                var pathEntry = request.PathEntries[i];
                cancellationToken.ThrowIfCancellationRequested();
                if (!TryGetFileContents(pathEntry.AbsolutePath, request.Output, out var fileContent))
                {
                    continue;
                }

                // Create a new entry with the original file content.
                var entry = CreateEntry(offset, fileContent, pathEntry.RelativePath);

                // Apply encryption when a key is provided and encryption is successful.
                if (key.Length > 0 && _aesEncryptionService.EncryptCbc(fileContent, out var output, key,
                        request.ProgressSecondary, request.ProgressReportInterval, cancellationToken))
                {
                    fileContent = output;
                }

                // Write the file contents to the specified offset.
                binaryWriter.Seek(offset, SeekOrigin.Begin);
                binaryWriter.Write(fileContent);

                // Update the entry collection and offset.
                entries[i] = entry;
                offset += entry.PackSize;
                percentage = (int)((double)(i + 1) / entries.Length * 100);
            }

            return entries;
        }
    }
}