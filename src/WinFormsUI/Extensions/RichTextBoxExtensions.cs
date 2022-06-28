using System.Runtime.InteropServices;

namespace WinFormsUI.Extensions
{
    internal static class RichTextBoxExtensions
    {
        private const int SB_PAGEBOTTOM = 0x7;

        private const int WM_VSCROLL = 0x115;

        internal static void ScrollToBottom(this RichTextBox richTextBox)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(richTextBox.ScrollToBottom);
                return;
            }

            SendMessage(richTextBox.Handle, WM_VSCROLL, (IntPtr)SB_PAGEBOTTOM, IntPtr.Zero);
            richTextBox.SelectionStart = richTextBox.Text.Length;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
    }
}