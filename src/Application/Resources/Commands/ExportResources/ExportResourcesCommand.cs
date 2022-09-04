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

namespace ResourcePackerGUI.Application.Resources.Commands.ExportResources
{
    public record ExportResourcesCommand : IRequest
    {
        /// <summary>
        /// Constructor for the <see cref="ExportResourcesCommand"/> class.
        /// </summary>
        /// <param name="basePath">The root folder to write the resources to.</param>
        /// <param name="resources">The resources to export.</param>
        /// <param name="conflictingNameReplacements">A dictionary of resolve methods for conflicting file encounters.</param>
        public ExportResourcesCommand(string basePath, IReadOnlyList<Resource> resources,
            IReadOnlyDictionary<Resource, string>? conflictingNameReplacements = null)
        {
            Resources = resources;
            ConflictingNameReplacements = conflictingNameReplacements;
            BasePath = basePath;
        }

        /// <summary>
        /// A dictionary of file names for conflicting encounters.
        /// </summary>
        public IReadOnlyDictionary<Resource, string>? ConflictingNameReplacements { get; init; }

        /// <summary>
        /// The resources to export.
        /// </summary>
        public IReadOnlyList<Resource> Resources { get; init; }

        /// <summary>
        /// The root folder to write the resources to.
        /// </summary>
        public string BasePath { get; init; }

        /// <summary>
        /// An optional progress instance to keep track of the export process.
        /// </summary>
        public IProgress<float>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}