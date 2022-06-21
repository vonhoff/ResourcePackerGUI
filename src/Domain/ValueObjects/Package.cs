using ResourcePackerGUI.Domain.Common;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Domain.ValueObjects
{
    public class Package : ValueObject
    {
        public Package(PackageHeader header, IReadOnlyList<Entry> entries)
        {
            Header = header;
            Entries = entries;
        }

        public IReadOnlyList<Entry> Entries { get; init; }
        public PackageHeader Header { get; init; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Header;
            yield return Entries;
        }
    }
}