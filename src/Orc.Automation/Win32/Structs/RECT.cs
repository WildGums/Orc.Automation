namespace Orc.Automation.Win32;

using System;
using System.Runtime.InteropServices;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
internal struct RECT
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public RECT(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public int Width => Right - Left;

    public int Height => Bottom - Top;
}
