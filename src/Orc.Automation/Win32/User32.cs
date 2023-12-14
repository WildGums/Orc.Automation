namespace Orc.Automation.Win32;

using System.Runtime.InteropServices;
using System;

internal static class User32
{
    [DllImport("user32.dll")]
    public static extern IntPtr WindowFromPoint(POINT point);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint msg, UIntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags fuFlags, uint uTimeout, out UIntPtr lpdwResult);
  
    [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
    public static extern int GetSystemMetrics(int nIndex);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern int MapVirtualKey(int nVirtKey, int nMapType);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int SendInput(int nInputs, ref Input mi, int cbSize);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern short VkKeyScan(char ch);

    [DllImport("user32.dll", EntryPoint = "GetPhysicalCursorPos", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
#pragma warning disable SA1300
    public static extern bool GetPhysicalCursorPos(out POINT lpPoint);
#pragma warning restore SA1300

    [DllImport("user32.dll", CharSet = CharSet.None, SetLastError = true, EntryPoint = "ScreenToClient")]
    public static extern bool ScreenToClient(IntPtr hWnd, ref POINT point);
}
