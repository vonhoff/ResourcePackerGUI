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

namespace ResourcePackerGUI.Application.Packaging.Queries.GetPackageInformation
{
    public record GetPackageInformationQuery : IRequest<Package>
    {
        /// <summary>
        /// Constructor for the <see cref="GetPackageInformationQuery"/> class.
        /// </summary>
        /// <param name="binaryReader">The binary reader to retrieve the package information.</param>
        public GetPackageInformationQuery(BinaryReader binaryReader)
        {
            BinaryReader = binaryReader;
        }

        /// <summary>
        /// The binary reader to retrieve the package information.
        /// </summary>
        public BinaryReader BinaryReader { get; init; }

        /// <summary>
        /// An optional progress instance to keep track of the amount of entries being loaded.
        /// </summary>
        public IProgress<int>? Progress { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instance when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}