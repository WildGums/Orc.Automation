namespace Orc.Automation;

using System;
using System.Windows;
using System.Windows.Interop;
using Win32;

public static class DpiHelper
{
    public static double GetDpi()
    {
        var resHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        var actualHeight = SystemParameters.PrimaryScreenHeight;
        var dpi = resHeight / actualHeight;

        return dpi;
    }

    public static Point DevicePixelsToLogical(POINT devicePoint, IntPtr hwnd)
    {
        return DevicePixelsToLogical(new Point(devicePoint.X, devicePoint.Y), hwnd);
    }

    public static Point DevicePixelsToLogical(Point devicePoint, IntPtr hwnd)
    {
        var hwndSource = HwndSource.FromHwnd(hwnd);

        return hwndSource?.CompositionTarget is null
            ? devicePoint
            : hwndSource.CompositionTarget.TransformFromDevice.Transform(devicePoint);
    }
}
