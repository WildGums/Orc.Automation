namespace Orc.Automation;

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using Win32;

/// <summary>
/// Exposes a simple interface to common mouse operations, allowing the user to simulate mouse input.
/// </summary>
public static class MouseInput
{
    /// <summary>
    /// Clicks a mouse button.
    /// </summary>
    /// <param name="mouseButton">The mouse button to click.</param>
    public static void Click(MouseButton mouseButton = MouseButton.Left)
    {
        Down(mouseButton);
        Up(mouseButton);
    }

    /// <summary>
    /// Double-clicks a mouse button.
    /// </summary>
    /// <param name="mouseButton">The mouse button to click.</param>
    public static void DoubleClick(MouseButton mouseButton = MouseButton.Left)
    {
        Click(mouseButton);
        Click(mouseButton);
    }

    /// <summary>
    /// Performs a mouse-down operation for a specified mouse button.
    /// </summary>
    /// <param name="mouseButton">The mouse button to use.</param>
    public static void Down(MouseButton mouseButton = MouseButton.Left)
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                SendMouseInput(0, 0, 0, SendMouseInputFlags.LeftDown);
                break;
            case MouseButton.Right:
                SendMouseInput(0, 0, 0, SendMouseInputFlags.RightDown);
                break;
            case MouseButton.Middle:
                SendMouseInput(0, 0, 0, SendMouseInputFlags.MiddleDown);
                break;
            case MouseButton.XButton1:
                SendMouseInput(0, 0, MouseInputTypes.XButton1, SendMouseInputFlags.XDown);
                break;
            case MouseButton.XButton2:
                SendMouseInput(0, 0, MouseInputTypes.XButton2, SendMouseInputFlags.XDown);
                break;
            default:
                throw new InvalidOperationException("Unsupported MouseButton input.");
        }
    }

    public static void Move(double deltaX, double deltaY)
    {
        User32.GetCursorPos(out var point);
        var newPoint = new Point(point.X + deltaX, point.Y + deltaY);

        MoveTo(newPoint);
    }

    /// <summary>
    /// Moves the mouse pointer to the specified screen coordinates.
    /// </summary>
    /// <param name="point">The screen coordinates to move to.</param>
    public static void MoveTo(Point point)
    {
        SendMouseInput(point.X, point.Y, 0, SendMouseInputFlags.Move | SendMouseInputFlags.Absolute);
    }

    /// <summary>
    /// Resets the system mouse to a clean state.
    /// </summary>
    public static void Reset()
    {
        MoveTo(new Point(0, 0));

        if (Mouse.LeftButton == MouseButtonState.Pressed)
        {
            SendMouseInput(0, 0, 0, SendMouseInputFlags.LeftUp);
        }

        if (Mouse.MiddleButton == MouseButtonState.Pressed)
        {
            SendMouseInput(0, 0, 0, SendMouseInputFlags.MiddleUp);
        }

        if (Mouse.RightButton == MouseButtonState.Pressed)
        {
            SendMouseInput(0, 0, 0, SendMouseInputFlags.RightUp);
        }

        if (Mouse.XButton1 == MouseButtonState.Pressed)
        {
            SendMouseInput(0, 0, MouseInputTypes.XButton1, SendMouseInputFlags.XUp);
        }

        if (Mouse.XButton2 == MouseButtonState.Pressed)
        {
            SendMouseInput(0, 0, MouseInputTypes.XButton2, SendMouseInputFlags.XUp);
        }
    }

    /// <summary>
    /// Simulates scrolling of the mouse wheel up or down.
    /// </summary>
    /// <param name="lines">The number of lines to scroll. Use positive numbers to scroll up and negative numbers to scroll down.</param>
    public static void Scroll(double lines)
    {
        var amount = (int)(MouseInputTypes.WheelDelta * lines);

        SendMouseInput(0, 0, amount, SendMouseInputFlags.Wheel);
    }

    /// <summary>
    /// Performs a mouse-up operation for a specified mouse button.
    /// </summary>
    /// <param name="mouseButton">The mouse button to use.</param>
    public static void Up(MouseButton mouseButton = MouseButton.Left)
    {
        switch (mouseButton)
        {
            case MouseButton.Left:
                SendMouseInput(0, 0, 0, SendMouseInputFlags.LeftUp);
                break;
            case MouseButton.Right:
                SendMouseInput(0, 0, 0, SendMouseInputFlags.RightUp);
                break;
            case MouseButton.Middle:
                SendMouseInput(0, 0, 0, SendMouseInputFlags.MiddleUp);
                break;
            case MouseButton.XButton1:
                SendMouseInput(0, 0, MouseInputTypes.XButton1, SendMouseInputFlags.XUp);
                break;
            case MouseButton.XButton2:
                SendMouseInput(0, 0, MouseInputTypes.XButton2, SendMouseInputFlags.XUp);
                break;
            default:
                throw new InvalidOperationException("Unsupported MouseButton input.");
        }
    }

    /// <summary>
    /// Sends mouse input.
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="data">scroll wheel amount</param>
    /// <param name="flags">SendMouseInputFlags flags</param>

    private static void SendMouseInput(double x, double y, int data, SendMouseInputFlags flags)
    {
        var intflags = (int)flags;

        if ((intflags & (int)SendMouseInputFlags.Absolute) != 0)
        {
            // Absolute position requires normalized coordinates.
            NormalizeCoordinates(ref x, ref y);
            intflags |= MouseEvents.Virtualdesk;
        }

        var mi = new Input
        {
            type = InputTypes.Mouse
        };
        mi.union.mouseInput.dx = (int)x;
        mi.union.mouseInput.dy = (int)y;
        mi.union.mouseInput.mouseData = data;
        mi.union.mouseInput.dwFlags = intflags;
        mi.union.mouseInput.time = 0;
        mi.union.mouseInput.dwExtraInfo = new IntPtr(0);

        if (User32.SendInput(1, ref mi, Marshal.SizeOf(mi)) == 0)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    private static void NormalizeCoordinates(ref double x, ref double y)
    {
        var vScreenWidth = User32.GetSystemMetrics(VirtualScreens.SmCx);
        var vScreenHeight = User32.GetSystemMetrics(VirtualScreens.SmCy);
        var vScreenLeft = User32.GetSystemMetrics(VirtualScreens.SmX);
        var vScreenTop = User32.GetSystemMetrics(VirtualScreens.SmY);

        // Absolute input requires that input is in 'normalized' coords - with the entire
        // desktop being (0,0)...(65536,65536). Need to convert input x,y coords to this
        // first.
        //
        // In this normalized world, any pixel on the screen corresponds to a block of values
        // of normalized coords - eg. on a 1024x768 screen,
        // y pixel 0 corresponds to range 0 to 85.333,
        // y pixel 1 corresponds to range 85.333 to 170.666,
        // y pixel 2 correpsonds to range 170.666 to 256 - and so on.
        // Doing basic scaling math - (x-top)*65536/Width - gets us the start of the range.
        // However, because int math is used, this can end up being rounded into the wrong
        // pixel. For example, if we wanted pixel 1, we'd get 85.333, but that comes out as
        // 85 as an int, which falls into pixel 0's range - and that's where the pointer goes.
        // To avoid this, we add on half-a-"screen pixel"'s worth of normalized coords - to
        // push us into the middle of any given pixel's range - that's the 65536/(Width*2)
        // part of the formula. So now pixel 1 maps to 85+42 = 127 - which is comfortably
        // in the middle of that pixel's block.
        // The key ting here is that unlike points in coordinate geometry, pixels take up
        // space, so are often better treated like rectangles - and if you want to target
        // a particular pixel, target its rectangle's midpoint, not its edge.
        x = (x - vScreenLeft) * 65536d / vScreenWidth /*+ 65536d / (vScreenWidth * 2)*/;
        y = (y - vScreenTop) * 65536d / vScreenHeight /*+ 65536d / (vScreenHeight * 2)*/;
    }
}
