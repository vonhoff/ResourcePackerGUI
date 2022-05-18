using Serilog;

namespace ResourcePacker.Extensions
{
    public static class ByteExtensions
    {
        /// <summary>
        /// Converts byte array into a <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="blob">The byte array.</param>
        /// <returns>A bitmap from the provided byte array.</returns>
        public static Bitmap? ToBitmap(this byte[] blob)
        {
            var memoryStream = new MemoryStream();
            memoryStream.Write(blob, 0, blob.Length);
            try
            {
                var bitmap = new Bitmap(memoryStream, false);
                memoryStream.Dispose();
                return bitmap;
            }
            catch (Exception exception)
            {
                Log.Error("Could not convert byte array to bitmap: \n{exception}", exception);
                return null;
            }
        }
    }
}