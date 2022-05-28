using System.Runtime.InteropServices;

namespace ResourcePacker.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Entry
    {
        public uint Id;
        public uint Crc;
        public int DataSize;
        public int PackSize;
        public uint Offset;
    }
}