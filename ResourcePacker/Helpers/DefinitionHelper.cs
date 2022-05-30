using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Force.Crc32;
using Serilog;

namespace ResourcePacker.Helpers
{
    internal static class DefinitionHelper
    {
        private static readonly Regex ValidDefinitionRegex =
            new(@"^((\.\./|[a-zA-Z0-9_/\-\\ ])*\.[a-zA-Z0-9]+)$", RegexOptions.Compiled);

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
                if (!ValidDefinitionRegex.IsMatch(definition))
                {
                    Log.Error("Invalid definition: {@entry}",
                        new { Definition = definition });
                    continue;
                }

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

            return crcDictionary;
        }
    }
}