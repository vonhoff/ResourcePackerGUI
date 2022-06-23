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