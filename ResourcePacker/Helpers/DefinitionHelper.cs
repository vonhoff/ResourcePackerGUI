using System.Text;
using Force.Crc32;
using Serilog;

namespace ResourcePacker.Helpers
{
    internal static class DefinitionHelper
    {
        public static Dictionary<uint, string> Create(Stream fileStream)
        {
            var definitions = new Dictionary<uint, string>();

            using var reader = new StreamReader(fileStream);
            while (!reader.EndOfStream)
            {
                var value = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                value = value.Replace(@"\", "/").ToLowerInvariant();
                var bytes = Encoding.ASCII.GetBytes(value);
                var crc = Crc32Algorithm.Compute(bytes);

                if (!definitions.ContainsKey(crc))
                {
                    definitions.Add(Crc32Algorithm.Compute(bytes), value);
                    continue;
                }

                Log.Warning("Duplicate definition: {entry}", value);
            }

            return definitions;
        }
    }
}