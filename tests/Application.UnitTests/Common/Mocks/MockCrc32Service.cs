using System.Text;
using Application.UnitTests.Common.Utilities;
using ResourcePackerGUI.Application.Common.Interfaces;

namespace Application.UnitTests.Common.Mocks
{
    internal class MockCrc32Service : ICrc32Service
    {
        private static readonly IReadOnlyDictionary<int, uint> CrcTableVerbose = new Dictionary<int, uint>
        {
            {2067372615, 2082955709},
            {1298366978, 1449897466},
            {954356668, 2909368490},
            {-1941208743, 3855723112},
            {572129015, 1449897466},
            {-551561264, 2909368490},
            {930796405, 3855723112}
        };

        private static readonly IReadOnlyDictionary<int, uint> CrcTable = new Dictionary<int, uint>
        {
            {524863851, 3090263786},
            {56576831, 3106401826},
            {102587468, 497522232},
            {-889950859, 3411133111},
            {383146337, 859863405},
            {-983369527, 1556428786},
            {1460893620, 1255023149},
            {1845234259, 1637759593},
            {-907827636, 4053477474},
        };

        public uint Compute(byte[] input, int offset, int length)
        {
            var hash = FnvHash.Compute(input.Select(v => (byte)(v + offset + length)).ToArray());
            if (CrcTableVerbose.TryGetValue(hash, out var result))
            {
                return result;
            }

            throw new InvalidDataException();
        }

        public uint Compute(byte[] input)
        {
            var hash = FnvHash.Compute(input);
            if (CrcTable.TryGetValue(hash, out var result))
            {
                return result;
            }

            throw new InvalidDataException();
        }
    }
}