using System.Runtime.InteropServices;

namespace ResourcePacker.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Entry
    {
        public uint Id;
        public uint Crc;
        public uint DataSize;
        public uint PackSize;
        public uint Offset;
    }
}