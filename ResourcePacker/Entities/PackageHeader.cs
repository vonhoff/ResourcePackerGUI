using System.Runtime.InteropServices;

namespace ResourcePacker.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    public struct PackageHeader
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Id;

        public int Reserved;

        public int NumberOfEntries;
    }
}