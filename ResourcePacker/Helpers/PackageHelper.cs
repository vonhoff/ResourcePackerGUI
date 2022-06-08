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

using System.IO.Packaging;
using System.Runtime.InteropServices;
using ResourcePacker.Entities;
using Serilog;

namespace ResourcePacker.Helpers
{
    internal static class PackageHelper
    {
        public static string PackHeaderId => "ResPack";

        public static void Build(HashSet<string> items, int relativeDepth, string packageOutput, 
            string password, string definitionOutput = "", IProgress<(int percentage, string path)>? progress = null)
        {
            var relativePaths = DefinitionHelper.CreateDefinitions(items, relativeDepth,
                definitionOutput, progress, 20);

            
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