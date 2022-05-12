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
                
                value = value.Replace(@"\", "/").Trim().ToLowerInvariant();
                if (!ValidDefinitionRegex.IsMatch(value))
                {
                    Log.Error("Invalid definition: {entry}", value);
                    continue;
                }

                var bytes = Encoding.ASCII.GetBytes(value);
                var crc = Crc32Algorithm.Compute(bytes);

                if (!definitions.ContainsKey(crc))
                {
                    Log.Debug("Added definition: {entry}", value);
                    definitions.Add(Crc32Algorithm.Compute(bytes), value);
                    continue;
                }

                Log.Warning("Duplicate definition: {entry}", value);
            }

            return definitions;
        }

        public static List<string>? LoadDefinitions(Stream fileStream)
        {
            var definitions = new List<string>();
            var invalidDefinitionAmount = 0;
            using var reader = new StreamReader(fileStream);
            while (!reader.EndOfStream)
            {
                var value = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                value = value.Replace(@"\", "/").Trim().ToLowerInvariant();
                if (!ValidDefinitionRegex.IsMatch(value))
                {
                    Log.Error("Invalid definition: {entry}", value);
                    invalidDefinitionAmount++;
                    continue;
                }

                if (definitions.Contains(value))
                {
                    Log.Warning("Duplicate definition: {entry}", value);
                    continue;
                }

                Log.Debug("Added definition: {entry}", value);
                definitions.Add(value);
            }

            if (definitions.Count == 0)
            {
                MessageBox.Show("The specified file does not contain any valid file definitions. \r\n" +
                                "A definition file should only contain the file paths of the files contained in the package.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return definitions;
        }
    }
}