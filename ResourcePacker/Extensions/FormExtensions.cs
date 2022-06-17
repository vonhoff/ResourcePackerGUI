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

using System.Media;
using System.Runtime.InteropServices;

namespace ResourcePacker.Extensions
{
    internal static class FormExtensions
    {
        // Flash both the window caption and taskbar button.
        // This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags.
        private const uint FLASHW_ALL = 3;

        // Flash continuously until the window comes to the foreground.
        private const uint FLASHW_TIMERNOFG = 12;

        /// <summary>
        /// Send a form taskbar notification, the window will blink until it gets focus.
        /// <remarks>
        /// This method allows a window to flash to let the user know that an important event has occurred
        /// in the application that requires their attention.
        /// </remarks>
        /// </summary>
        internal static bool FlashNotification(this Form form)
        {
            if (form.InvokeRequired)
            {
                return form.Invoke(() => FlashNotification(form));
            }

            if (ApplicationIsActivated())
            {
                return false;
            }

            var hWnd = form.Handle;
            var fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
            fInfo.uCount = uint.MaxValue;
            fInfo.dwTimeout = 0;

            return FlashWindowEx(ref fInfo);
        }

        private static bool ApplicationIsActivated()
        {
            var activatedHandle = GetForegroundWindow();

            // No window is currently activated
            if (activatedHandle == IntPtr.Zero)
            {
                return false;
            }

            GetWindowThreadProcessId(activatedHandle, out var processId);
            return processId == Environment.ProcessId;
        }

        // To support flashing.
        [DllImport("user32.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public uint cbSize;
            public IntPtr hwnd;
            public uint dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }
    }
}