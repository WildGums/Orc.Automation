﻿namespace Orc.Automation.Win32;

using System.Runtime.InteropServices;

internal static class Kernel32
{
    [DllImport("kernel32.dll")]
    public static extern bool LoadLibraryA(string hModule);
}
