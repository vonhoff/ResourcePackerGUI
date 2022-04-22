using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ResourcePacker.Entities;

namespace ResourcePacker.Helpers
{
    internal static class PackHelper
    {
        public static string PackHeaderId => "ResPack";

        public static Pack Open(Stream fileStream, string? password)
        {
            var header = GetHeader(fileStream);
            if (!header.Id.Equals(PackHeaderId))
            {
                throw new InvalidDataException("The specified file is not a valid resource package.");
            }

            //var packSize = Marshal.SizeOf(typeof(Pack)) +
            //               (Marshal.SizeOf(typeof(Entry)) * header.NumberOfEntries);

            // todo: Create pack from stream
            return new Pack();
        }

        /// <summary>
        /// Retrieves header information as <see cref="PackHeader"/> from the provided data stream.
        /// </summary>
        /// <param name="fileStream">The stream of data.</param>
        /// <returns>Header information as <see cref="PackHeader"/> from <paramref name="fileStream"/></returns>
        public static PackHeader GetHeader(Stream fileStream)
        {
            var headerSize = Marshal.SizeOf(typeof(PackHeader));
            var binaryReader = new BinaryReader(fileStream);
            var buffer = binaryReader.ReadBytes(headerSize);
            var ptr = Marshal.AllocHGlobal(headerSize);

            Marshal.Copy(buffer, 0, ptr, headerSize);
            var structure = Marshal.PtrToStructure(ptr, typeof(PackHeader));

            Marshal.FreeHGlobal(ptr);
            return structure != null ? (PackHeader)structure : default;
        }
    }
}
