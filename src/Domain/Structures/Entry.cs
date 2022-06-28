using System.Runtime.InteropServices;

namespace ResourcePackerGUI.Domain.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Entry
    {
        public uint Id;
        public uint Crc;
        public int DataSize;
        public int PackSize;
        public int Offset;
    }
}