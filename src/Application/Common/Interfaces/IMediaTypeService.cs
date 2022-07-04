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

using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Common.Interfaces
{
    public interface IMediaTypeService
    {
        /// <summary>
        /// Attempts to retrieve a <see cref="MediaType"/> by the provided byte array.
        /// </summary>
        /// <param name="data">The byte array.</param>
        /// <returns>A <see cref="MediaType"/> when successful, <see langword="null"/> otherwise.</returns>
        MediaType? GetTypeByData(byte[] data);

        /// <summary>
        /// Attempts to retrieve a <see cref="MediaType"/> by the name extension.
        /// </summary>
        /// <param name="name">The name to get the media type from.</param>
        /// <returns>A <see cref="MediaType"/> when successful, <see langword="null"/> otherwise.</returns>
        MediaType? GetTypeByName(string name);
    }
}