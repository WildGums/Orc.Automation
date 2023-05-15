namespace Orc.Automation.Win32;

using System.Runtime.InteropServices;
using System;

public static class User32
{
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags fuFlags, uint uTimeout, out UIntPtr lpdwResult);
}
