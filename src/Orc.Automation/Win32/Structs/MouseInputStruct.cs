namespace Orc.Automation.Win32;

using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal struct MouseInputStruct
{
    public int dx;
    public int dy;
    public int mouseData;
    public int dwFlags;
    public int time;
    public IntPtr dwExtraInfo;
};
