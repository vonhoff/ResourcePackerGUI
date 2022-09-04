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
using ResourcePackerGUI.Domain.Structures;

namespace ResourcePackerGUI.Application.Packaging.Queries.GetPackageResources
{
    public record GetPackageResourcesQuery : IRequest<IReadOnlyList<Resource>>
    {
        /// <summary>
        /// Constructor for the <see cref="GetPackageResourcesQuery"/> class.
        /// </summary>
        /// <param name="entries">The collection of entries for retrieving all resources.</param>
        /// <param name="binaryReader">The binary reader for the specified package file.</param>
        /// <param name="password">The password for use to decrypt the resources.</param>
        public GetPackageResourcesQuery(IReadOnlyList<Entry> entries, BinaryReader binaryReader, string password = "")
        {
            Entries = entries;
            BinaryReader = binaryReader;
            Password = password;
        }

        /// <summary>
        /// The collection of entries for retrieving all resources.
        /// </summary>
        public IReadOnlyList<Entry> Entries { get; init; }

        /// <summary>
        /// The binary reader for the specified package file.
        /// </summary>
        public BinaryReader BinaryReader { get; init; }

        /// <summary>
        /// The password for use to decrypt the resources.
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// An optional progress to keep track of the amount of resources loaded.
        /// </summary>
        public IProgress<float>? ProgressPrimary { get; init; }

        /// <summary>
        /// An optional progress to keep track of the decryption process.
        /// </summary>
        public IProgress<float>? ProgressSecondary { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}