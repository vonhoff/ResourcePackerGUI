using System.Runtime.InteropServices;

namespace ResourcePacker.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PackHeader
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Id;

        public uint Reserved;

        public uint NumberOfEntries;
    }
}