﻿#region GNU General Public License

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
        private static readonly int PlatformWordSize = IntPtr.Size;

        /// <summary>
        /// CopyMemory is faster than Buffer.BlockCopy when the byte count is not greater than 128 bytes.
        /// </summary>
        /// <param name="src">Source</param>
        /// <param name="srcOff">Source offset</param>
        /// <param name="dst">Destination</param>
        /// <param name="dstOff">Destination offset</param>
        /// <param name="count">Byte count</param>
        public static void CopyMemory(byte[] src, int srcOff, byte[] dst, int dstOff, int count)
        {
            unsafe
            {
                fixed (byte* srcPtr = &src[srcOff])
                {
                    fixed (byte* dstPtr = &dst[dstOff])
                    {
                        CopyMemory(srcPtr, dstPtr, count);
                    }
                }
            }
        }

        private static unsafe void CopyMemory(byte* srcPtr, byte* dstPtr, int count)
        {
            const int u32Size = sizeof(uint);
            const int u64Size = sizeof(ulong);
            const int u128Size = sizeof(ulong) << 1;

            var srcEndPtr = srcPtr + count;

            switch (PlatformWordSize)
            {
                case u32Size:
                {
                    // 32-bit
                    while (srcPtr + u64Size <= srcEndPtr)
                    {
                        *(uint*)dstPtr = *(uint*)srcPtr;
                        dstPtr += u32Size;
                        srcPtr += u32Size;
                        *(uint*)dstPtr = *(uint*)srcPtr;
                        dstPtr += u32Size;
                        srcPtr += u32Size;
                    }

                    break;
                }
                case u64Size:
                {
                    // 64-bit
                    while (srcPtr + u128Size <= srcEndPtr)
                    {
                        *(ulong*)dstPtr = *(ulong*)srcPtr;
                        dstPtr += u64Size;
                        srcPtr += u64Size;
                        *(ulong*)dstPtr = *(ulong*)srcPtr;
                        dstPtr += u64Size;
                        srcPtr += u64Size;
                    }

                    if (srcPtr + u64Size <= srcEndPtr)
                    {
                        *(ulong*)dstPtr ^= *(ulong*)srcPtr;
                        dstPtr += u64Size;
                        srcPtr += u64Size;
                    }

                    break;
                }
            }

            if (srcPtr + u32Size <= srcEndPtr)
            {
                *(uint*)dstPtr = *(uint*)srcPtr;
                dstPtr += u32Size;
                srcPtr += u32Size;
            }

            if (srcPtr + sizeof(ushort) <= srcEndPtr)
            {
                *(ushort*)dstPtr = *(ushort*)srcPtr;
                dstPtr += sizeof(ushort);
                srcPtr += sizeof(ushort);
            }

            if (srcPtr + 1 <= srcEndPtr)
            {
                *dstPtr = *srcPtr;
            }
        }

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