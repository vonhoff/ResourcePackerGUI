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

namespace ResourcePackerGUI.Domain.Entities
{
    public record PathEntry
    {
        /// <summary>
        /// Constructor for the <see cref="PathEntry"/> class.
        /// </summary>
        /// <param name="absolutePath">The full path towards the file location.</param>
        /// <param name="relativePath">The relative path from the specified root folder.</param>
        public PathEntry(string absolutePath, string relativePath)
        {
            AbsolutePath = absolutePath;
            RelativePath = relativePath;
        }

        /// <summary>
        /// The full path towards the file location.
        /// </summary>
        public string AbsolutePath { get; set; }

        /// <summary>
        /// The relative path from the specified root folder.
        /// </summary>
        public string RelativePath { get; set; }
    }
}