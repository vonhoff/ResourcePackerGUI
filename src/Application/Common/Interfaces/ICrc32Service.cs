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

namespace ResourcePackerGUI.Application.Common.Interfaces
{
    public interface ICrc32Service
    {
        /// <summary>Computes CRC-32 from the input buffer.</summary>
        /// <param name="input">Input buffer with data to be check summed.</param>
        /// <param name="offset">Offset of the input data within the buffer.</param>
        /// <param name="length">Length of the input data in the buffer.</param>
        /// <returns>CRC-32 of the data in the buffer.</returns>
        uint Compute(byte[] input, int offset, int length);

        /// <summary>Computes CRC-32 from the input buffer.</summary>
        /// <param name="input">Input buffer containing data to be check summed.</param>
        /// <returns>CRC-32 of the buffer.</returns>
        uint Compute(byte[] input);
    }
}