using ResourcePacker.Entities;
using System.Runtime.InteropServices;

namespace ResourcePacker.Helpers;

internal static class PackHelper
{
    public static string PackHeaderId => "ResPack";

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

    public static Pack Load(PackHeader header, Stream fileStream, CancellationToken cancellationToken = default)
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
            pack.Entries[i] = Marshal.PtrToStructure<Entry>(ins);
        }

        return pack;
    }
}