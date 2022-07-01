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

using Serilog;

namespace WinFormsUI.Helpers
{
    public static class DirectoryHelper
    {
        /// <summary>
        /// Attempts to get all files from a path and skip inaccessible files or folders.
        /// </summary>
        /// <param name="path">The path to enumerate.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to cancel the operation.</param>
        /// <returns>A <see cref="IEnumerable{T}"/> of all accessible files.</returns>
        public static IEnumerable<string> GetAllFiles(string path, CancellationToken cancellationToken)
        {
            return Directory.EnumerateFiles(path).Union(
                Directory.EnumerateDirectories(path).SelectMany(d =>
                {
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                        return GetAllFiles(d, cancellationToken);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Log.Warning(ex, "Could not access directory: {path}", d);
                        return Enumerable.Empty<string>();
                    }
                }));
        }
    }
}