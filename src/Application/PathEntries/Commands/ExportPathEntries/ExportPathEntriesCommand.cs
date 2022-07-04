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
using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.PathEntries.Commands.ExportPathEntries
{
    public record ExportPathEntriesCommand : IRequest
    {
        /// <summary>
        /// Constructor for the <see cref="ExportPathEntriesCommand"/> class.
        /// </summary>
        /// <param name="pathEntries">The path entries to export.</param>
        /// <param name="output">The output file to write the entries to.</param>
        public ExportPathEntriesCommand(IReadOnlyList<PathEntry> pathEntries, string output)
        {
            PathEntries = pathEntries;
            Output = output;
        }

        /// <summary>
        /// The path entries to export.
        /// </summary>
        public IReadOnlyList<PathEntry> PathEntries { get; init; }

        /// <summary>
        /// The output file location to write to.
        /// </summary>
        public string Output { get; init; }

        /// <summary>
        /// An optional progress instance to keep track of the writing process.
        /// </summary>
        public IProgress<int>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}