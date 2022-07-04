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

namespace ResourcePackerGUI.Application.Resources.Queries.GetConflictingResources
{
    public record GetConflictingResourcesQuery : IRequest<IReadOnlyList<Resource>>
    {
        /// <summary>
        /// Constructor for the <see cref="GetConflictingResourcesQuery"/> class.
        /// </summary>
        /// <param name="basePath">The base folder to check for conflicts.</param>
        /// <param name="resources">The collection of resources.</param>
        public GetConflictingResourcesQuery(string basePath, IReadOnlyList<Resource> resources)
        {
            BasePath = basePath;
            Resources = resources;
        }

        /// <summary>
        /// The root folder to check for conflicts.
        /// </summary>
        public string BasePath { get; init; }

        /// <summary>
        /// The collection of resources to check if a file
        /// with the same name already exists on the file system.
        /// </summary>
        public IReadOnlyList<Resource> Resources { get; init; }

        /// <summary>
        /// An optional progress instance to keep track of the checking process.
        /// </summary>
        public IProgress<int>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}