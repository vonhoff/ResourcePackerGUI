using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ResourcePacker.Entities;

namespace ResourcePacker.Helpers;

internal static class PackHelper
{
    public static string PackHeaderId => "ResPack";

    public static Pack Open(Stream fileStream, string? password)
    {
        var binaryReader = new BinaryReader(fileStream);

        var header = GetHeader(binaryReader);
        if (!header.Id.Equals(PackHeaderId) || header.NumberOfEntries <= 0)
        {
            throw new InvalidDataException("The specified file is invalid.");
        }

        var entrySize = Marshal.SizeOf(typeof(Entry));
        var packSize = Marshal.SizeOf(typeof(Pack)) + (entrySize * header.NumberOfEntries);
        var ptr = Marshal.AllocHGlobal(packSize);

        var buffer = binaryReader.ReadBytes(packSize);

        Marshal.Copy(buffer, 0, ptr, packSize);

        var pack = new Pack
        {
            FileStream = fileStream,
            NumberOfEntries = header.NumberOfEntries,
            Entries = new Entry[header.NumberOfEntries]
        };

        for (var i = 0; i < pack.NumberOfEntries; ++i)
        {
            var ins = new IntPtr(ptr.ToInt64() + (i * entrySize));
            pack.Entries[i] = Marshal.PtrToStructure<Entry>(ins);
        }

        return pack;
    }

    /// <summary>
    /// Retrieves header information as <see cref="PackHeader"/> from the provided data stream.
    /// </summary>
    /// <param name="binaryReader">A binary reader containing the stream of data.</param>
    /// <returns>Header information as <see cref="PackHeader"/> from <paramref name="fileStream"/></returns>
    public static PackHeader GetHeader(BinaryReader binaryReader)
    {
        var headerSize = Marshal.SizeOf(typeof(PackHeader));
        var ptr = Marshal.AllocHGlobal(headerSize);
        var buffer = binaryReader.ReadBytes(headerSize);
        
        Marshal.Copy(buffer, 0, ptr, headerSize);
        var structure = Marshal.PtrToStructure(ptr, typeof(PackHeader));

        Marshal.FreeHGlobal(ptr);
        return structure != null ? (PackHeader)structure : default;
    }
}