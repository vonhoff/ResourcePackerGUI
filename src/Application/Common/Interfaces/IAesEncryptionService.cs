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
    public interface IAesEncryptionService
    {
        /// <summary>
        /// Decrypts the provided input using the cipher block chaining mode.
        /// </summary>
        /// <param name="input">The input to decrypt.</param>
        /// <param name="dataSize">The size of the original data.</param>
        /// <param name="output">The resulting output.</param>
        /// <param name="key">The key for decrypting the specified <paramref name="input"/>.</param>
        /// <param name="progress">An optional progress to keep track of the decryption process.</param>
        /// <param name="progressReportInterval">The interval in milliseconds for updating the progress instances when present.</param>
        /// <param name="cancellationToken">A cancellation token to cancel the decryption process.</param>
        /// <returns><see langword="true"/> when successful, <see langword="false"/> otherwise.</returns>
        bool DecryptCbc(byte[] input, int dataSize, out byte[] output, uint[] key, IProgress<int>? progress = null,
            int progressReportInterval = 100, CancellationToken cancellationToken = default);

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
        bool EncryptCbc(byte[] input, out byte[] output, uint[] key, IProgress<int>? progress = null,
            int progressReportInterval = 100, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates a key for use with the cipher methods.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>A key consisting of unsigned integers.</returns>
        uint[] KeySetup(string password);
    }
}