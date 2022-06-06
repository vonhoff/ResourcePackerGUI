#region GNU General Public License

/* Copyright 2022 Simon Vonhoff
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

using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace ResourcePacker.Helpers
{
    public static class DirectoryHelper
    {
        private const uint FileAttributeDirectory = 16;
        private const uint FileAttributeNormal = 128;

        public static bool CheckDirectoryEmpty(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(path);
            }

            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException();
            }

            path += path.EndsWith(Path.DirectorySeparatorChar.ToString()) ?
                "*" : Path.DirectorySeparatorChar + "*";

            var findHandle = FindFirstFile(path, out var findData);

            if (findHandle == IntPtr.Zero)
            {
                throw new Exception("Cannot find the first file in the folder.",
                    Marshal.GetExceptionForHR(Marshal.GetHRForLastWin32Error()));
            }

            try
            {
                var empty = true;
                do
                {
                    if (findData.cFileName != "." &&
                        findData.cFileName != ".." &&
                        findData.dwFileAttributes is FileAttributeDirectory or FileAttributeNormal)
                    {
                        empty = false;
                    }
                } while (empty && FindNextFile(findHandle, out findData));

                return empty;
            }
            finally
            {
                FindClose(findHandle);
            }
        }

        [DllImport("kernel32.dll")]
        private static extern bool FindClose(IntPtr hFindFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindFirstFile(string lpFileName, out Win32FindData lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FindNextFile(IntPtr hFindFile, out Win32FindData lpFindFileData);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct Win32FindData
        {
            public readonly uint dwFileAttributes;
            private readonly FILETIME ftCreationTime;
            private readonly FILETIME ftLastAccessTime;
            private readonly FILETIME ftLastWriteTime;
            private readonly uint nFileSizeHigh;
            private readonly uint nFileSizeLow;
            private readonly uint dwReserved0;
            private readonly uint dwReserved1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public readonly string cFileName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            private readonly string cAlternateFileName;
        }
    }
}