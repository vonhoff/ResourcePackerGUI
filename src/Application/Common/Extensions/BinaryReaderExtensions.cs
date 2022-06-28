using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ResourcePackerGUI.Application.Common.Extensions
{
    internal static class BinaryReaderExtensions
    {
        /// <summary>
        /// Reads a <see cref="T"/> instance from a <see cref="BinaryReader"/> instance.
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
        /// Writes a <see cref="T"/> instance to a <see cref="BinaryWriter"/> instance.
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