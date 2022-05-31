using System.IO;
using System.Text;
using Force.Crc32;
using Serilog;

namespace ResourcePacker.Helpers
{
    internal static class DefinitionHelper
    {
        /// <summary>
        /// Creates a dictionary of names and CRC codes.
        /// </summary>
        /// <param name="definitionStream">The stream which contains the definitions.</param>
        /// <returns>A dictionary of names and CRC codes.</returns>
        public static IReadOnlyDictionary<uint, string> CreateCrcDictionary(Stream definitionStream)
        {
            var crcDictionary = new Dictionary<uint, string>();
            using var reader = new StreamReader(definitionStream);
            while (!reader.EndOfStream)
            {
                var definition = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(definition))
                {
                    continue;
                }

                definition = definition.Replace(@"\", "/").Trim().ToLowerInvariant();
                var bytes = Encoding.ASCII.GetBytes(definition);
                var crc = Crc32Algorithm.Compute(bytes);

                if (!crcDictionary.ContainsKey(crc))
                {
                    Log.Debug("Added definition: {@entry}",
                        new { Id = crc, Definition = definition });
                    crcDictionary.Add(crc, definition);
                    continue;
                }

                Log.Warning("Duplicate definition: {@entry}",
                    new { Id = crc, Definition = definition });
            }

            Log.Information("Created {definitionCount} definitions.", crcDictionary.Count);
            return crcDictionary;
        }
    }
}