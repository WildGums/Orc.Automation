namespace Orc.Automation
{
    using System.Windows;

    public static class DpiHelper
    {
        public static double GetDpi()
        {
            var resHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
            var actualHeight = SystemParameters.PrimaryScreenHeight;
            var dpi = resHeight / actualHeight;

            return dpi;
        }
    }
}
