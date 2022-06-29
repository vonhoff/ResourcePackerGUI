using Force.Crc32;
using ResourcePackerGUI.Application.Common.Interfaces;

namespace ResourcePackerGUI.Infrastructure.Services
{
    public class Crc32Service : ICrc32Service
    {
        public uint Compute(byte[] input, int offset, int length)
        {
            return Crc32Algorithm.Compute(input, offset, length);
        }

        public uint Compute(byte[] input)
        {
            return Crc32Algorithm.Compute(input);
        }
    }
}