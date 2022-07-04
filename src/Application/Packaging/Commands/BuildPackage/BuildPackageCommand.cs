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

namespace ResourcePackerGUI.Application.Packaging.Commands.BuildPackage
{
    public record BuildPackageCommand : IRequest
    {
        /// <summary>
        /// Constructor for the <see cref="BuildPackageCommand"/> class.
        /// </summary>
        /// <param name="pathEntries">A read-only collection of <see cref="PathEntry"/> instances.</param>
        /// <param name="output">The destination of the package file.</param>
        /// <param name="password">An optional password used to encrypt the data located at the path entries.</param>
        public BuildPackageCommand(IReadOnlyList<PathEntry> pathEntries, string output, string password = "")
        {
            PathEntries = pathEntries;
            Output = output;
            Password = password;
        }

        /// <summary>
        /// The destination of the package file.
        /// </summary>
        public string Output { get; init; }

        /// <summary>
        /// The password used to encrypt the data located at the path entries.
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// A read-only collection of <see cref="PathEntry"/> instances.
        /// </summary>
        public IReadOnlyList<PathEntry> PathEntries { get; init; }

        /// <summary>
        /// An optional secondary progress instance to keep track of the encryption progress.
        /// </summary>
        public IProgress<int>? ProgressSecondary { get; init; }

        /// <summary>
        /// An optional primary progress instance to keep track of the amount of files processed.
        /// </summary>
        public IProgress<int>? ProgressPrimary { get; init; }

        /// <summary>
        /// The interval in milliseconds for updating the progress instances when present.
        /// </summary>
        public int ProgressReportInterval { get; init; } = 100;
    }
}