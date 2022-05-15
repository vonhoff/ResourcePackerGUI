using System.Text;
using System.Text.RegularExpressions;
using Force.Crc32;
using ResourcePacker.Entities;
using Serilog;

namespace ResourcePacker.Helpers
{
    internal static class DefinitionHelper
    {
        private static readonly Regex ValidDefinitionRegex =
            new(@"^((\.\./|[a-zA-Z0-9_/\-\\ ])*\.[a-zA-Z0-9]+)$", RegexOptions.Compiled);

        public static List<EntryDefinition> CreateEntryDefinitions(Pack package)
        {
            return package.Entries.Select(entry => new EntryDefinition { Entry = entry }).ToList();
        }

        public static List<EntryDefinition> CreateEntryDefinitions(Pack package, Stream definitionStream)
        {
            var crcDictionary = CreateCrcDictionary(definitionStream);
            var definitions = new List<EntryDefinition>();

            foreach (var entry in package.Entries)
            {
                if (crcDictionary.TryGetValue(entry.Id, out var filePath))
                {
                    Log.Debug("Found definition for entry: {@id}", new { entry.Id });
                    definitions.Add(new EntryDefinition { Entry = entry, Name = filePath });
                    continue;
                }

                definitions.Add(new EntryDefinition { Entry = entry });
                Log.Warning("Could not find definition for entry: {@id}", new { entry.Id });
            }

            return definitions;
        }

        private static IReadOnlyDictionary<uint, string> CreateCrcDictionary(Stream definitionStream)
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