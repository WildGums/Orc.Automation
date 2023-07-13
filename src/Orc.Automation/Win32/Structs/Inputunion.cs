namespace Orc.Automation.Win32;

using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit)]
internal struct Inputunion
{
    [FieldOffset(0)]
    public MouseInputStruct mouseInput;
    [FieldOffset(0)]
    public Keybdinput keyboardInput;
};
