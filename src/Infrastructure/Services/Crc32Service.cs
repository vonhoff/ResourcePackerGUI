using Force.Crc32;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Infrastructure.Utilities;

namespace ResourcePackerGUI.Infrastructure.Services
{
    public class Crc32Service : ICrc32Service
    {
        public uint Compute(byte[] input, int offset, int length)
        {
            var r = Crc32Algorithm.Compute(input, offset, length);

            using var stream = File.Open("G:\\Predefined\\crcv.txt", FileMode.Append);
            using StreamWriter writer = new(stream);
            writer.WriteLine("{"+ FnvHash.Compute(input.Select(v => (byte)(v + offset + length)).ToArray()) + ", "+r+"}");
            return r;
        }

        public uint Compute(byte[] input)
        {
            var r = Crc32Algorithm.Compute(input);

            using var stream = File.Open("G:\\Predefined\\crc.txt", FileMode.Append);
            using StreamWriter writer = new(stream);
            writer.WriteLine("{" + FnvHash.Compute(input) + ", " + r + "}");
            return r;
        }
    }
}