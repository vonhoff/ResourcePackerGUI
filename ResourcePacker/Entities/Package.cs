using System.Runtime.InteropServices;

namespace ResourcePacker.Entities
{
    public class Package
    {
        public Stream FileStream;

        public int NumberOfEntries;

        public Entry[] Entries;
    }
}