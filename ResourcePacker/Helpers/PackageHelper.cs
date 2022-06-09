#region GNU General Public License

/* Copyright 2022 Simon Vonhoff
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

using System.Diagnostics;
using System.IO.Packaging;
using System.Runtime.InteropServices;
using System.Text;
using Force.Crc32;
using ResourcePacker.Entities;
using Serilog;

namespace ResourcePacker.Helpers
{
    internal static class PackageHelper
    {
        public static string PackHeaderId => "ResPack";

        public static void Build(IReadOnlyList<string> items, int relativeDepth, string packageOutput,
            string password, string definitionOutput = "", IProgress<(int percentage, string path)>? progress = null)
        {
            var paths = DefinitionHelper.CreateDefinitionFile(items, relativeDepth,
                definitionOutput, progress, 30);

            var header = new PackageHeader
            {
                Id = PackHeaderId,
                NumberOfEntries = paths.Count
            };

            var outputStream = File.Open(packageOutput, FileMode.CreateNew);
            var binaryWriter = new BinaryWriter(outputStream);

            var key = AesEncryptionHelper.KeySetup(password);
            var entries = new Entry[header.NumberOfEntries];
            var offset = Marshal.SizeOf(typeof(PackageHeader)) + (header.NumberOfEntries * Marshal.SizeOf(typeof(Entry)));
            var entryIndex = 0;

            foreach (var (absolutePath, relativePath) in paths)
            {
                byte[] fileContent;
                try
                {
                    fileContent = File.ReadAllBytes(absolutePath);
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Could not open file stream for {path}", absolutePath);
                    continue;
                }

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
                    var packSize = (dataSize + AesEncryptionHelper.BlockSize - 1) & ~(AesEncryptionHelper.BlockSize - 1);
                    if (packSize == dataSize)
                    {
                        packSize += AesEncryptionHelper.BlockSize;
                    }

                    entry.PackSize = packSize;

                    var output = new byte[packSize];
                    var pkcs7 = packSize - dataSize;
                    var dataToEncrypt = fileContent.Concat(
                        Enumerable.Repeat((byte)pkcs7, pkcs7).ToArray()).ToArray();

                    if (!AesEncryptionHelper.EncryptCbc(dataToEncrypt, packSize, ref output, key))
                    {
                        continue;
                    }

                    fileContent = output;
                }

                binaryWriter.Seek(offset, SeekOrigin.Begin);
                binaryWriter.Write(fileContent);
                offset += entry.PackSize;
            }

            binaryWriter.Seek(0, SeekOrigin.Begin);
            binaryWriter.Write(header);
        }

        /// <summary>
        /// Gets the header of the provided file stream.
        /// </summary>
        /// <param name="fileStream">The stream of the provided file.</param>
        /// <returns>The header of the provided resource package.</returns>
        /// <exception cref="InvalidDataException">
        /// When the file stream is not a valid <see langword="ResPack"/> stream.</exception>
        public static PackageHeader GetHeader(Stream fileStream)
        {
            var binaryReader = new BinaryReader(fileStream);
            var headerSize = Marshal.SizeOf(typeof(PackageHeader));

            var buffer = binaryReader.ReadBytes(headerSize);
            var ptr = Marshal.AllocHGlobal(headerSize);

            Marshal.Copy(buffer, 0, ptr, headerSize);
            var structure = Marshal.PtrToStructure(ptr, typeof(PackageHeader));

            Marshal.FreeHGlobal(ptr);
            var header = structure != null ? (PackageHeader)structure : default;

            if (header.Id != null && (!header.Id.Equals(PackHeaderId) || header.NumberOfEntries <= 0))
            {
                throw new InvalidDataException("The specified file is invalid.");
            }

            return header;
        }

        /// <summary>
        /// Creates a package containing all information about the entries inside a package.
        /// </summary>
        /// <param name="header">A <see cref="PackageHeader"/> instance.</param>
        /// <param name="fileStream">The stream of the package file.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Package"/> instance containing all information about a package.</returns>
        /// <exception cref="InvalidDataException">When the provided file stream is corrupted.</exception>
        public static Entry[] LoadAllEntryInformation(PackageHeader header, Stream fileStream, CancellationToken cancellationToken = default)
        {
            var binaryReader = new BinaryReader(fileStream);
            var entrySize = Marshal.SizeOf(typeof(Entry));
            var packSize = Marshal.SizeOf(typeof(PackageHeader)) + (entrySize * header.NumberOfEntries);

            var buffer = binaryReader.ReadBytes(packSize);
            var ptr = Marshal.AllocHGlobal(packSize);

            Marshal.Copy(buffer, 0, ptr, packSize);

            var entries = new List<Entry>();
            for (var i = 0; i < header.NumberOfEntries; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var ins = new IntPtr(ptr.ToInt64() + (i * entrySize));
                var entry = Marshal.PtrToStructure<Entry>(ins);

                if (entry.Id == 0)
                {
                    Log.Error("Invalid entry: {@entry}",
                        new { entry.Id, entry.Crc, entry.DataSize, entry.PackSize });
                    continue;
                }

                entries.Add(entry);
                Log.Debug("Added entry: {@entry}",
                    new { entry.Id, entry.Crc, entry.DataSize });
            }

            if (entries.Count == header.NumberOfEntries)
            {
                Log.Information("Loaded {entryCount} entries.", entries.Count);
            }
            else
            {
                Log.Warning("Loaded {entryCount} out of {expectedCount} entries.",
                    entries.Count, header.NumberOfEntries);
            }

            return entries.ToArray();
        }
    }
}