#nullable enable
namespace Orc.Automation.Tests;

using System.Drawing;
using System.Windows;
using System.Drawing.Imaging;

public static class ScreenshotHelper
{
    public static Bitmap? CaptureScreen(Rect bounds)
    {
        try
        {
            var captureBitmap = new Bitmap((int)bounds.Width, (int)bounds.Height, PixelFormat.Format32bppArgb);
            var captureGraphics = Graphics.FromImage(captureBitmap);
            captureGraphics.CopyFromScreen((int)bounds.X, (int)bounds.Y, 0, 0, new System.Drawing.Size((int)bounds.Width, (int)bounds.Height));

            captureGraphics.Dispose();

            return captureBitmap;
        }
        catch
        {
            return null;
        }
    }
}
