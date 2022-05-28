using System.Runtime.InteropServices;
using ResourcePacker.Entities;
using Serilog;

namespace ResourcePacker.Helpers;

internal static class PackHelper
{
    public static string PackHeaderId => "ResPack";

    /// <summary>
    /// Gets the header of the provided file stream.
    /// </summary>
    /// <param name="fileStream">The stream of the provided file.</param>
    /// <returns>The header of the provided resource package.</returns>
    /// <exception cref="InvalidDataException">
    /// When the file stream is not a valid <see langword="ResPack"/> stream.</exception>
    public static PackHeader GetHeader(Stream fileStream)
    {
        var binaryReader = new BinaryReader(fileStream);
        var headerSize = Marshal.SizeOf(typeof(PackHeader));

        var buffer = binaryReader.ReadBytes(headerSize);
        var ptr = Marshal.AllocHGlobal(headerSize);

        Marshal.Copy(buffer, 0, ptr, headerSize);
        var structure = Marshal.PtrToStructure(ptr, typeof(PackHeader));

        Marshal.FreeHGlobal(ptr);
        var header = structure != null ? (PackHeader)structure : default;

        if (header.Id != null && (!header.Id.Equals(PackHeaderId) || header.NumberOfEntries <= 0))
        {
            throw new InvalidDataException("The specified file is invalid.");
        }

        return header;
    }

    /// <summary>
    /// Creates a package containing all information about the entries inside a package.
    /// </summary>
    /// <param name="header">A <see cref="PackHeader"/> instance.</param>
    /// <param name="fileStream">The stream of the package file.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A <see cref="Pack"/> instance containing all information about a package.</returns>
    /// <exception cref="InvalidDataException">When the provided file stream is corrupted.</exception>
    public static Pack LoadAllEntryInformation(PackHeader header, Stream fileStream, CancellationToken cancellationToken = default)
    {
        var binaryReader = new BinaryReader(fileStream);
        var entrySize = Marshal.SizeOf(typeof(Entry));
        var packSize = Marshal.SizeOf(typeof(Pack)) + (entrySize * header.NumberOfEntries);

        var buffer = binaryReader.ReadBytes(packSize);
        var ptr = Marshal.AllocHGlobal(packSize);

        Marshal.Copy(buffer, 0, ptr, packSize);

        var pack = new Pack
        {
            FileStream = fileStream,
            NumberOfEntries = header.NumberOfEntries,
            Entries = new Entry[header.NumberOfEntries]
        };

        for (var i = 0; i < pack.NumberOfEntries; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var ins = new IntPtr(ptr.ToInt64() + (i * entrySize));
            var entry = Marshal.PtrToStructure<Entry>(ins);

            if (entry.Crc == 0 || entry.DataSize == 0 || entry.Id == 0 || entry.PackSize == 0)
            {
                Log.Warning("Invalid entry: {@entry}",
                    new { entry.Id, entry.Crc, entry.DataSize, entry.PackSize });
            }

            pack.Entries[i] = entry;
            Log.Debug("Added entry: {@entry}",
                new { entry.Id, entry.Crc, entry.DataSize });
        }

        return pack;
    }
}