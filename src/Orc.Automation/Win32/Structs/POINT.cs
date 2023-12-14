namespace Orc.Automation.Win32;

using System;
using System.Runtime.InteropServices;
using System.Windows;

/// <summary>
/// Struct representing a point.
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Sequential)]
// ReSharper disable once InconsistentNaming
internal struct POINT
{
    public int X;
    public int Y;

    public static implicit operator Point(POINT point)
    {
        return new Point(point.X, point.Y);
    }
}
