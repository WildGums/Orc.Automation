namespace Orc.Automation.Win32;

using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal struct Keybdinput
{
    public short wVk;
    public short wScan;
    public int dwFlags;
    public int time;
    public IntPtr dwExtraInfo;
};
