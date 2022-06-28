using System.Text;
using MediatR;
using ResourcePackerGUI.Application.Common.Interfaces;
using ResourcePackerGUI.Application.Definitions.Queries;
using Serilog;

namespace ResourcePackerGUI.Application.Definitions.Handlers
{
    public class CreateChecksumDefinitionsQueryHandler : IRequestHandler<CreateChecksumDefinitionsQuery, IReadOnlyDictionary<uint, string>>
    {
        private readonly ICrc32Service _crc32Service;

        public CreateChecksumDefinitionsQueryHandler(ICrc32Service crc32Service)
        {
            _crc32Service = crc32Service;
        }

        public Task<IReadOnlyDictionary<uint, string>> Handle(CreateChecksumDefinitionsQuery request, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                IReadOnlyDictionary<uint, string> crcDictionary;
                using (var reader = new StreamReader(request.FileStream))
                {
                    crcDictionary = CreateChecksumDictionary(reader, cancellationToken);
                }

                Log.Information("Computed {definitionCount} checksum definitions.", crcDictionary.Count);
                return crcDictionary;
            }, cancellationToken);
        }

        /// <summary>
        /// Creates a checksum dictionary of checksum values and their original entries.
        /// </summary>
        /// <param name="reader">The stream reader containing all definition entries.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the stream reading process.</param>
        /// <returns>A read-only dictionary of checksum values and their original entries.</returns>
        private IReadOnlyDictionary<uint, string> CreateChecksumDictionary(StreamReader reader, CancellationToken cancellationToken)
        {
            var crcDictionary = new Dictionary<uint, string>();
            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var definition = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(definition))
                {
                    continue;
                }

                definition = NormalizeFilePath(definition);
                var bytes = Encoding.ASCII.GetBytes(definition);
                var crc = _crc32Service.Compute(bytes);

                if (!crcDictionary.ContainsKey(crc))
                {
                    Log.Debug("Added checksum definition: {@entry}", new { Id = crc, Definition = definition });
                    crcDictionary.Add(crc, definition);
                    continue;
                }

                Log.Warning("Duplicate checksum definition: {@entry}", new { Id = crc, Definition = definition });
            }

            return crcDictionary;
        }

        /// <summary>
        /// Normalizes the provided definition by replacing all backslashes with forward slashes, <br/>
        /// trimming the text, and setting all text to lowercase.
        /// </summary>
        /// <param name="definition">The text to normalize.</param>
        /// <returns>The normalized text.</returns>
        private static string NormalizeFilePath(string definition)
        {
            return definition.Replace(@"\", "/").Trim().ToLowerInvariant();
        }
    }
}