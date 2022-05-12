﻿using System.Runtime.InteropServices;

namespace ResourcePacker.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PackHeader
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Id;

        public int Reserved;

        public int NumberOfEntries;
    }
}