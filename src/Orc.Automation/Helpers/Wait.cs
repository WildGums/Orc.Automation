namespace Orc.Automation;

using System;
using System.Threading;
using System.Windows.Automation;
using Catel.IoC;
using Services;
using Win32;

public static class Wait
{
    public static bool WhileProcessBusy(int waitTime = 100)
    {
        var process = ServiceLocator.Default.ResolveType<ISetupAutomationService>()?.CurrentSetup?.Process;
        return process?.WaitForInputIdle(waitTime) == true;
    }

    public static void UntilInputProcessed(int waitTime = 100)
    {
        if (waitTime > 0)
        {
            Thread.Sleep(waitTime);
        }
    }

    /// <summary>
    /// Wait main window to be responsive after manipulations
    /// </summary>
    public static void UntilResponsive(int waitTime = 100)
    {
        if (waitTime <= 0)
        {
            return;
        }

        var mainWindow = ServiceLocator.Default.ResolveType<ISetupAutomationService>()?.CurrentSetup
            ?.MainWindow;

        if (mainWindow is null)
        {
            Thread.Sleep(waitTime);

            return;
        }

        UntilResponsive((IntPtr)mainWindow.Current.NativeWindowHandle, TimeSpan.FromMilliseconds(waitTime));
    }

    public static bool UntilResponsive(IntPtr hWnd, TimeSpan timeout)
    {
        var ret = User32.SendMessageTimeout(hWnd, WindowsMessages.WM_NULL,
            UIntPtr.Zero, IntPtr.Zero, SendMessageTimeoutFlags.SMTO_NORMAL, (uint)timeout.TotalMilliseconds, out _);
        // There might be other things going on so do a small sleep anyway...
        // Other sources: http://blogs.msdn.com/b/oldnewthing/archive/2014/02/13/10499047.aspx
        Thread.Sleep(20);
        return ret != IntPtr.Zero;
    }
}
