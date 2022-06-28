using Force.Crc32;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Infrastructure.Utilities;

namespace ResourcePackerGUI.Infrastructure.Services
{
    public class Crc32Service : ICrc32Service
    {
        public uint Compute(byte[] input, int offset, int length)
        {
            using var file = File.Open("F:\\predefined\\crc32v.txt", FileMode.Append);
            using var writer = new StreamWriter(file);
            writer.Write("{");
            writer.Write(FnvHash.Compute(input.Concat(new[] { (byte)offset, (byte)length }).ToArray()));
            writer.Write(", ");

            var x = Crc32Algorithm.Compute(input, offset, length);
            writer.Write(x);
            writer.WriteLine("},");
            return x;
        }

        public uint Compute(byte[] input)
        {
            using var file = File.Open("F:\\predefined\\crc32.txt", FileMode.Append);
            using var writer = new StreamWriter(file);
            writer.Write("{");
            writer.Write(FnvHash.Compute(input));
            writer.Write(", ");

            var x = Crc32Algorithm.Compute(input);
            writer.Write(x);
            writer.WriteLine("},");
            return x;
        }
    }
}