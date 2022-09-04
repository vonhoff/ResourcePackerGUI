#region GNU General Public License

/* Copyright 2022 Simon Vonhoff & Contributors
 *
 * This file is part of ResourcePackerGUI.
 *
 * ResourcePackerGUI is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation, either version 3 of the License,
 * or (at your option) any later version.
 *
 * ResourcePackerGUI is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 * See the GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License along with ResourcePackerGUI.
 * If not, see <https://www.gnu.org/licenses/>.
 */

#endregion

using System.Text;
using MediatR;
using ResourcePackerGUI.Application.Common.Interfaces;
using Serilog;

namespace ResourcePackerGUI.Application.Definitions.Queries.CreateChecksumDefinitions
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
            IReadOnlyDictionary<uint, string> crcDictionary;

            using var progressTimer = new System.Timers.Timer(request.ProgressReportInterval);
            var percentage = 0f;
            // ReSharper disable once AccessToModifiedClosure
            progressTimer.Elapsed += delegate { request.Progress!.Report(percentage); };
            progressTimer.Enabled = request.Progress != null;

            using (var reader = new StreamReader(request.FileStream))
            {
                crcDictionary = CreateChecksumDictionary(reader, ref percentage, cancellationToken);
                Log.Information("Computed {count} checksum definitions.", crcDictionary.Count);
            }

            request.Progress?.Report(100);
            return Task.FromResult(crcDictionary);
        }

        /// <summary>
        /// Creates a checksum dictionary of checksum values and their original entries.
        /// </summary>
        /// <param name="reader">The stream reader containing all definition entries.</param>
        /// <param name="percentage">The percentage of the total progress.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the stream reading process.</param>
        /// <returns>A read-only dictionary of checksum values and their original entries.</returns>
        private IReadOnlyDictionary<uint, string> CreateChecksumDictionary(StreamReader reader, ref float percentage, CancellationToken cancellationToken)
        {
            var crcDictionary = new Dictionary<uint, string>();
            var entries = new List<string>();

            while (!reader.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var line = reader.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    entries.Add(line);
                }
            }

            for (var i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                var definition = entry.Replace(@"\", "/").Trim().ToLowerInvariant();
                var bytes = Encoding.ASCII.GetBytes(definition);
                var crc = _crc32Service.Compute(bytes);

                if (!crcDictionary.ContainsKey(crc))
                {
                    Log.Debug("Added checksum definition: {@entry}", new { Id = crc, Definition = definition });
                    crcDictionary.Add(crc, definition);
                    continue;
                }

                Log.Warning("Duplicate checksum definition: {@entry}", new { Id = crc, Definition = definition });
                percentage = (float)(i + 1) / entries.Count * 100f;
            }

            return crcDictionary;
        }
    }
}