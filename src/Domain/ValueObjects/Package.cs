using ResourcePackerGUI.Domain.Common;
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Domain.ValueObjects
{
    public class Package : ValueObject
    {
        public PackageHeader Header { get; init; }

        public IReadOnlyList<Entry> Entries { get; init; }

        public Package(PackageHeader header, IReadOnlyList<Entry> entries)
        {
            Header = header;
            Entries = entries;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Header;
            yield return Entries;
        }
    }
}