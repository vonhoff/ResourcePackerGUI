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

namespace ResourcePackerGUI.Application.PathEntries.Queries.CreatePathEntries
{
    public record CreatePathEntriesQuery : IRequest<IReadOnlyList<PathEntry>>
    {
        /// <summary>
        /// Constructor for the <see cref="CreatePathEntriesQuery"/> class.
        /// </summary>
        /// <param name="absoluteFilePaths">A list of absolute file paths to create relative paths from.</param>
        /// <param name="relativeFilePathDepth">The relative file path depth.</param>
        public CreatePathEntriesQuery(IReadOnlyList<string> absoluteFilePaths, int relativeFilePathDepth)
        {
            AbsoluteFilePaths = absoluteFilePaths;
            RelativeFilePathDepth = relativeFilePathDepth;
        }

        /// <summary>
        /// A list of absolute file paths to create relative paths from.
        /// </summary>
        public IReadOnlyList<string> AbsoluteFilePaths { get; init; }

        /// <summary>
        /// The relative file path depth.
        /// </summary>
        public int RelativeFilePathDepth { get; init; }

        /// <summary>
        /// An optional progress instance to keep track of the path creation process.
        /// </summary>
        public IProgress<int>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}