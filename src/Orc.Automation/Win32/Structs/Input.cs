namespace Orc.Automation.Win32;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
internal struct Input
{
    public int type;
    public Inputunion union;
};
