namespace ResourcePackerGUI.Application.Common.Interfaces
{
    public interface IAesEncryptionService
    {
        /// <summary>
        /// Decrypts the provided input using the cipher block chaining mode.
        /// </summary>
        /// <param name="input">The input to decrypt.</param>
        /// <param name="output">The resulting output.</param>
        /// <param name="key">The key for decrypting the specified <paramref name="input"/>.</param>
        /// <param name="progress">An optional progress to keep track of the decryption process.</param>
        /// <param name="progressReportInterval">The interval in milliseconds for updating the progress instances when present.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the decryption process.</param>
        /// <returns><see langword="true"/> when successful, <see langword="false"/> otherwise.</returns>
        bool DecryptCbc(byte[] input, out byte[] output, uint[] key, IProgress<int>? progress,
            int progressReportInterval, CancellationToken cancellationToken);

        /// <summary>
        /// Encrypts the provided input using the cipher block chaining mode.
        /// </summary>
        /// <param name="input">The input to encrypt.</param>
        /// <param name="output">The resulting output.</param>
        /// <param name="key">The key for encrypting the specified <paramref name="input"/>.</param>
        /// <param name="progress">An optional progress to keep track of the encryption process.</param>
        /// <param name="progressReportInterval">The interval in milliseconds for updating the progress instances when present.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the encryption process.</param>
        /// <returns><see langword="true"/> when successful, <see langword="false"/> otherwise.</returns>
        bool EncryptCbc(byte[] input, out byte[] output, uint[] key, IProgress<int>? progress,
            int progressReportInterval, CancellationToken cancellationToken);

        /// <summary>
        /// Creates a key for use with the cipher methods.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>A key consisting of unsigned integers.</returns>
        uint[] KeySetup(string password);
    }
}