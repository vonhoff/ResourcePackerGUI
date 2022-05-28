﻿using System.Runtime.InteropServices;

namespace ResourcePacker.Entities
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Pack
    {
        public Stream FileStream;

        public byte[] Key;

        public int NumberOfEntries;

        public Entry[] Entries;

        public bool Encrypted;
    }
}