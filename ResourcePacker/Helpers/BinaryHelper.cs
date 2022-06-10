#region GNU General Public License

/* Copyright 2022 Vonhoff, MaxtorCoder
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

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ResourcePacker.Helpers
{
    public static class BinaryHelper
    {
        /// <summary>
        /// Reads a <see cref="T"/> instance from the <see cref="BinaryReader"/> instance.
        /// </summary>
        /// <typeparam name="T">The <see cref="T"/> instance to be read.</typeparam>
        /// <param name="reader">The <see cref="BinaryReader"/> instance.</param>
        /// <returns>The <see cref="T"/> instance.</returns>
        public static T ReadStruct<T>(this BinaryReader reader) where T : struct
        {
            var result = reader.ReadBytes(Unsafe.SizeOf<T>());
            return Unsafe.ReadUnaligned<T>(ref result[0]);
        }

        /// <summary>
        /// Writes a <see cref="T"/> instance to the <see cref="BinaryWriter"/> instance.
        /// </summary>
        /// <typeparam name="T">The <see cref="T"/> instance to be written.</typeparam>
        /// <param name="writer">The <see cref="BinaryWriter"/> instance.</param>
        /// <param name="instance">The <see cref="T"/> instance to write.</param>
        public static void WriteStruct<T>(this BinaryWriter writer, T instance) where T : struct
        {
            var span = MemoryMarshal.CreateSpan(ref instance, 1);
            writer.Write(MemoryMarshal.AsBytes(span));
        }
    }
}