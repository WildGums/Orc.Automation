namespace Orc.Automation.Win32;

using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Win32;

[Serializable]
[StructLayout(LayoutKind.Sequential)]
public struct WINDOWPLACEMENT
{
    [XmlIgnore]
    public int Length;
    [XmlIgnore]
    public int Flags;
    public int ShowCmd;
    [XmlIgnore]
    public POINT MinPosition;
    [XmlIgnore]
    public POINT MaxPosition;
    public RECT NormalPosition;
}
