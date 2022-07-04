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

using Force.Crc32;
using ResourcePackerGUI.Application.Common.Interfaces;

namespace ResourcePackerGUI.Infrastructure.Services
{
    public class Crc32Service : ICrc32Service
    {
        public uint Compute(byte[] input, int offset, int length)
        {
            return Crc32Algorithm.Compute(input, offset, length);
        }

        public uint Compute(byte[] input)
        {
            return Crc32Algorithm.Compute(input);
        }
    }
}