#region GNU General Public License

/* Copyright 2022 Vonhoff, MaxtorCoder
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

using System.IO.Packaging;
using System.Runtime.CompilerServices;
using System.Text;
using Force.Crc32;
using ResourcePacker.Entities;
using ResourcePacker.Extensions;
using Serilog;

namespace ResourcePacker.Helpers
{
    internal static class PackageHelper
    {
        public static ulong PackHeaderId => 0x6B636150736552;

        /// <summary>
        /// Builds a resource package from a set of absolute and relative paths.
        /// </summary>
        /// <param name="paths">A dictionary of paths where the key is the absolute path and the value is relative.</param>
        /// <param name="packageOutput">The destination of the resource package.</param>
        /// <param name="password">An optional password to be used during the encryption process.</param>
        /// <param name="progressPrimary">An optional progress to keep track of the amount of assets being packed.</param>
        /// <param name="progressSecondary">An optional progress to keep track of the encryption state.</param>
        /// <param name="progressReportInterval"></param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the operation.</param>
        public static void BuildPackage(IReadOnlyDictionary<string, string> paths, string packageOutput,
                    string password = "", IProgress<(int percentage, string path)>? progressPrimary = null,
                    IProgress<int>? progressSecondary = null, int progressReportInterval = 100,
                    CancellationToken cancellationToken = default)
        {
            var header = new PackageHeader
            {
                Id = PackHeaderId,
                NumberOfEntries = paths.Count
            };

            using var outputStream = File.Open(packageOutput, FileMode.OpenOrCreate);
            using var binaryWriter = new BinaryWriter(outputStream);

            var key = AesEncryptionHelper.KeySetup(password);
            var entryIndex = 0;
            var entries = new Entry[paths.Count];
            var offset = Unsafe.SizeOf<PackageHeader>() + (header.NumberOfEntries * Unsafe.SizeOf<Entry>());

            using (var progressTimer = new System.Timers.Timer(progressReportInterval))
            {
                var relativePath = string.Empty;
                var percentage = 0;
                progressTimer.Elapsed += delegate { progressPrimary!.Report((percentage, relativePath)); };
                progressTimer.Enabled = progressPrimary != null;

                foreach (var (absolutePath, name) in paths)
                {
                    relativePath = name;
                    percentage = (int)((double)(entryIndex) / entries.Length * 100);
                    cancellationToken.ThrowIfCancellationRequested();

                    byte[] fileContent;
                    try
                    {
                        if (absolutePath.Equals(packageOutput))
                        {
                            throw new InvalidOperationException("The specified file is the same as the output file.");
                        }

                        fileContent = File.ReadAllBytes(absolutePath);
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex, "Could not open file stream for {path}", absolutePath);
                        continue;
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    var dataSize = fileContent.Length;
                    var nameBytes = Encoding.ASCII.GetBytes(relativePath);
                    var nameCrc = Crc32Algorithm.Compute(nameBytes);
                    var fileCrc = Crc32Algorithm.Compute(fileContent);

                    var entry = new Entry
                    {
                        Id = nameCrc,
                        Crc = fileCrc,
                        Offset = offset,
                        DataSize = dataSize,
                        PackSize = dataSize
                    };

                    if (key.Length > 0)
                    {
                        var packSize = (dataSize + AesEncryptionHelper.BlockSize - 1) &
                                       ~(AesEncryptionHelper.BlockSize - 1);
                        if (packSize == dataSize)
                        {
                            packSize += AesEncryptionHelper.BlockSize;
                        }

                        entry.PackSize = packSize;

                        // Fill with PKCS#7 padding value.
                        var pkcs7 = packSize - dataSize;
                        var dataToEncrypt = fileContent.Concat(
                            Enumerable.Repeat((byte)pkcs7, pkcs7).ToArray()).ToArray();

                        var output = new byte[packSize];
                        if (AesEncryptionHelper.EncryptCbc(dataToEncrypt, packSize,
                                ref output, key, progressSecondary, progressReportInterval, cancellationToken))
                        {
                            fileContent = output;
                        }
                    }

                    binaryWriter.Seek(offset, SeekOrigin.Begin);
                    binaryWriter.Write(fileContent);
                    entries[entryIndex++] = entry;
                    offset += entry.PackSize;
                }
            }

            // Write header to file.
            binaryWriter.Seek(0, SeekOrigin.Begin);
            binaryWriter.WriteStruct(header);

            // Write entry metadata to file.
            foreach (var entry in entries)
            {
                cancellationToken.ThrowIfCancellationRequested();
                binaryWriter.WriteStruct(entry);
            }

            outputStream.Close();
        }

        /// <summary>
        /// Gets the header of the provided file stream.
        /// </summary>
        /// <param name="binaryReader">The binary reader for the provided file.</param>
        /// <returns>The header of the provided resource package.</returns>
        /// <exception cref="InvalidDataException">
        /// When the file stream is not a valid <see langword="ResPack"/> stream.</exception>
        public static PackageHeader GetHeader(BinaryReader binaryReader)
        {
            var header = binaryReader.ReadStruct<PackageHeader>();
            if (header.Id != PackHeaderId || header.NumberOfEntries <= 0)
            {
                throw new InvalidDataException("The specified file is invalid.");
            }

            return header;
        }

        /// <summary>
        /// Creates a package containing all information about the entries inside a package.
        /// </summary>
        /// <param name="header">A <see cref="PackageHeader"/> instance.</param>
        /// <param name="binaryReader">A reader for the package file.</param>
        /// <param name="progressReportInterval"></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="progress"></param>
        /// <returns>A <see cref="Package"/> instance containing all information about a package.</returns>
        /// <exception cref="InvalidDataException">When the provided file stream is corrupted.</exception>
        public static Entry[] LoadAllEntryInformation(PackageHeader header, BinaryReader binaryReader, 
            IProgress<int>? progress = null, int progressReportInterval = 100, CancellationToken cancellationToken = default)
        {
            var entries = new List<Entry>();
           
            using (var progressTimer = new System.Timers.Timer(progressReportInterval))
            {
                var percentage = 0;
                progressTimer.Elapsed += delegate { progress!.Report(percentage); };
                progressTimer.Enabled = progress != null;

                for (var i = 0; i < header.NumberOfEntries; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var entry = binaryReader.ReadStruct<Entry>();
                    if (entry.Id == 0)
                    {
                        Log.Warning("Invalid entry: {@entry}",
                            new { entry.Id, entry.Crc, entry.DataSize, entry.PackSize });
                        continue;
                    }

                    entries.Add(entry);
                    Log.Debug("Added entry: {@entry}",
                        new { entry.Id, entry.Crc, entry.DataSize });
                    percentage = (int)((double)(i + 1) / header.NumberOfEntries * 100);
                }
            }

            if (entries.Count == header.NumberOfEntries)
            {
                Log.Information("Loaded all {entryCount} entries.", entries.Count);
            }
            else
            {
                Log.Warning("Loaded {entryCount} out of {expectedCount} entries.",
                    entries.Count, header.NumberOfEntries);
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Loads assets from a specified package.
        /// </summary>
        /// <param name="entries">An array of entry metadata.</param>
        /// <param name="binaryReader">A binary reader for the package.</param>
        /// <param name="password">The password to be used for loading the assets.</param>
        /// <param name="progressPrimary">An optional progress for the amount of files to be loaded.</param>
        /// <param name="progressSecondary">An optional progress for the state of file decryption.</param>
        /// <param name="progressReportInterval"></param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the operation.</param>
        /// <returns>A list of loaded assets.</returns>
        public static List<Asset> LoadAssetsFromPackage(Entry[] entries, BinaryReader binaryReader, string password,
            IProgress<(int percentage, int amount)>? progressPrimary = null,
            IProgress<int>? progressSecondary = null, int progressReportInterval = 100,
            CancellationToken cancellationToken = default)
        {
            var assets = new List<Asset>();
            using (var progressTimer = new System.Timers.Timer(progressReportInterval))
            {
                var i = 0;
                var percentage = 0;
                progressTimer.Elapsed += delegate { progressPrimary!.Report((percentage, i)); };
                progressTimer.Enabled = progressPrimary != null;

                for (i = 0; i < entries.Length; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var entry = entries[i];
                    if (!LoadSingleFromPackage(binaryReader, entry, out var asset, password,
                            progressSecondary, progressReportInterval, cancellationToken))
                    {
                        Log.Error("Integrity check failed for entry: {id}", new { entry.Id });
                        continue;
                    }

                    Log.Debug("Added asset: {@asset}",
                        new { asset.Name, MediaType = asset.MimeType?.Name });
                    assets.Add(asset);
                    percentage = (int)((double)(i + 1) / entries.Length * 100);
                }
            }

            if (assets.Count == entries.Length)
            {
                Log.Information("Loaded all {assetCount} assets.", assets.Count);
            }
            else
            {
                Log.Warning("Loaded {assetCount} out of {entryCount} assets.",
                    assets.Count, entries.Length);
            }

            return assets;
        }

        /// <summary>
        /// Attempts to load a specified asset from a provided binary reader.
        /// </summary>
        /// <param name="binaryReader">The package reader.</param>
        /// <param name="entry">Information about the entry.</param>
        /// <param name="asset"></param>
        /// <param name="password"></param>
        /// <param name="progress"></param>
        /// <param name="progressReportInterval"></param>
        /// <param name="cancellationToken"></param>
        /// <returns><see langword="true"/> when integrity check succeeded; otherwise, <see langword="false"/>.</returns>
        public static bool LoadSingleFromPackage(BinaryReader binaryReader, Entry entry,
            out Asset asset, string password = "", IProgress<int>? progress = null,
            int progressReportInterval = 100, CancellationToken cancellationToken = default)
        {
            if (entry.Offset < 0)
            {
                asset = new Asset();
                return false;
            }

            binaryReader.BaseStream.Position = entry.Offset;
            var buffer = binaryReader.ReadBytes(entry.PackSize);

            if (!string.IsNullOrEmpty(password))
            {
                var key = AesEncryptionHelper.KeySetup(password);
                var output = new byte[entry.PackSize];
                AesEncryptionHelper.DecryptCbc(buffer, entry.PackSize, ref output, key,
                    progress, progressReportInterval, cancellationToken);
                buffer = output;
            }

            var crc = Crc32Algorithm.Compute(buffer, 0, entry.DataSize);
            asset = new Asset(buffer);

            if (entry.Crc != crc)
            {
                return false;
            }

            asset.MimeType = AssetHelper.GetMimeType(buffer);
            asset.Entry = entry;
            return true;
        }
    }
}