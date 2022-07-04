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

namespace ResourcePackerGUI.Application.Resources.Queries.UpdateResourceDefinitions
{
    public record UpdateResourceDefinitionsQuery : IRequest<int>
    {
        /// <summary>
        /// Constructor for the <see cref="UpdateResourceDefinitionsQuery"/> class.
        /// </summary>
        /// <param name="resources">The collection of resources to update.</param>
        /// <param name="checksumDefinitions">A read-only dictionary of checksum values and their original entries.</param>
        public UpdateResourceDefinitionsQuery(IReadOnlyList<Resource> resources, IReadOnlyDictionary<uint, string> checksumDefinitions)
        {
            Resources = resources;
            ChecksumDefinitions = checksumDefinitions;
        }

        /// <summary>
        /// The collection of resources to update according to the checksum dictionary.
        /// </summary>
        public IReadOnlyList<Resource> Resources { get; init; }

        /// <summary>
        /// A read-only dictionary of checksum values and their original entries.
        /// </summary>
        public IReadOnlyDictionary<uint, string> ChecksumDefinitions { get; init; }

        /// <summary>
        /// An optional progress instance to keep track of the process.
        /// </summary>
        public IProgress<int>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}