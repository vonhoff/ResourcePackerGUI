using ResourcePackerGUI.Domain.Entities;

namespace ResourcePackerGUI.Application.Common.Interfaces
{
    public interface IMimeTypeService
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