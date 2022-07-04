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

using MediatR;

namespace ResourcePackerGUI.Application.Definitions.Queries.CreateChecksumDefinitions
{
    public record CreateChecksumDefinitionsQuery : IRequest<IReadOnlyDictionary<uint, string>>
    {
        /// <summary>
        /// Constructor for the <see cref="CreateChecksumDefinitionsQuery"/> class.
        /// </summary>
        /// <param name="fileStream">The stream of the specified definition file.</param>
        public CreateChecksumDefinitionsQuery(Stream fileStream)
        {
            FileStream = fileStream;
        }

        /// <summary>
        /// The stream of the specified definition file.
        /// </summary>
        public Stream FileStream { get; init; }

        /// <summary>
        /// An optional progress to keep track of the process.
        /// </summary>
        public IProgress<int>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}