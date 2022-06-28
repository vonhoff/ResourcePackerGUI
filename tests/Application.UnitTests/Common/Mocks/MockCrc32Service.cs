using Application.UnitTests.Common.Utilities;
using ResourcePackerGUI.Application.Common.Interfaces;

namespace Application.UnitTests.Common.Mocks
{
    internal class MockCrc32Service : ICrc32Service
    {
        private static readonly Dictionary<int, uint> Crc32TableVerbose = new()
        {
            {376600873, 2082955709},
            {1196588606, 3855723112},
            {-1237826543, 1449897466},
            {2144434038, 2909368490}
        };

        private static readonly Dictionary<int, uint> Crc32Table = new()
        {
            {102587468, 497522232},
            {-889950859, 3411133111},
            {383146337, 859863405},
            {524863851, 3090263786},
            {56576831, 3106401826},
            {-983369527, 1556428786},
            {1460893620, 1255023149},
            {1845234259, 1637759593},
            {-907827636, 4053477474}
        };

        public uint Compute(byte[] input, int offset, int length)
        {
            var hash = FnvHash.Compute(input.Concat(new[] { (byte)offset, (byte)length }).ToArray());
            if (Crc32TableVerbose.TryGetValue(hash, out var result))
            {
                return result;
            }

            throw new InvalidOperationException("The value is not pre-calculated.");
        }

        public uint Compute(byte[] input)
        {
            var hash = FnvHash.Compute(input);
            if (Crc32Table.TryGetValue(hash, out var result))
            {
                return result;
            }

            throw new InvalidOperationException("The value is not pre-calculated.");
        }
    }
}