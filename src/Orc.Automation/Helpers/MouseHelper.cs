namespace Orc.Automation;

using System;
using System.Diagnostics;
using System.Windows;
using Win32;

public static class MouseHelper
{
    public static bool TryGetRelativeMousePosition(IntPtr hWnd, out Point point)
    {
        point = default;
        POINT devicePoint = default;

        var returnValue = hWnd != IntPtr.Zero
                          && TryGetPhysicalCursorPos(out devicePoint);

        if (returnValue)
        {
            User32.ScreenToClient(hWnd, ref devicePoint);
        }

        point = new Point(devicePoint.X, devicePoint.Y);

        return returnValue;
    }

    private static bool TryGetPhysicalCursorPos(out POINT pt)
    {
        var returnValue = User32.GetPhysicalCursorPos(out pt);
        // Sometimes Win32 will fail this call, such as if you are
        // not running in the interactive desktop. For example,
        // a secure screen saver may be running.
        if (!returnValue)
        {
            Debug.WriteLine("GetPhysicalCursorPos failed!");
            pt.X = 0;
            pt.Y = 0;
        }

        return returnValue;
    }
}
