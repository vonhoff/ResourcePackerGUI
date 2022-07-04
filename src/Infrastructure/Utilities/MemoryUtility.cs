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

namespace ResourcePackerGUI.Infrastructure.Utilities
{
    public static class MemoryUtility
    {
        private const int U16Size = sizeof(ushort);
        private const int U32Size = sizeof(uint);
        private const int U64Size = sizeof(ulong);
#if X64
        private const int U128Size = sizeof(ulong) << 1;
#endif

        /// <summary>
        /// Copies a specified number of bytes from a source array starting at a
        /// particular offset to a destination array starting at a particular offset.
        /// </summary>
        /// <param name="src">The source buffer.</param>
        /// <param name="srcOff">The zero-based byte offset into <paramref name="src"/>.</param>
        /// <param name="dst">The destination buffer.</param>
        /// <param name="dstOff">The zero-based byte offset into <paramref name="dst"/>.</param>
        /// <param name="count">The number of bytes to copy.</param>
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
            var srcEndPtr = srcPtr + count;
#if X64
            while (srcPtr + U128Size <= srcEndPtr)
            {
                *(ulong*)dstPtr = *(ulong*)srcPtr;
                dstPtr += U64Size;
                srcPtr += U64Size;
                *(ulong*)dstPtr = *(ulong*)srcPtr;
                dstPtr += U64Size;
                srcPtr += U64Size;
            }

            if (srcPtr + U64Size <= srcEndPtr)
            {
                *(ulong*)dstPtr ^= *(ulong*)srcPtr;
                dstPtr += U64Size;
                srcPtr += U64Size;
            }
#else
            while (srcPtr + U64Size <= srcEndPtr)
            {
                *(uint*)dstPtr = *(uint*)srcPtr;
                dstPtr += U32Size;
                srcPtr += U32Size;
                *(uint*)dstPtr = *(uint*)srcPtr;
                dstPtr += U32Size;
                srcPtr += U32Size;
            }
#endif
            if (srcPtr + U32Size <= srcEndPtr)
            {
                *(uint*)dstPtr = *(uint*)srcPtr;
                dstPtr += U32Size;
                srcPtr += U32Size;
            }

            if (srcPtr + U16Size <= srcEndPtr)
            {
                *(ushort*)dstPtr = *(ushort*)srcPtr;
                dstPtr += U16Size;
                srcPtr += U16Size;
            }

            if (srcPtr + 1 <= srcEndPtr)
            {
                *dstPtr = *srcPtr;
            }
        }
    }
}