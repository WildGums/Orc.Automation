namespace Orc.Automation.Controls;

using System.Windows.Automation;

[Control(ControlTypeName = nameof(ControlType.CheckBox))]
public class CheckBox : FrameworkElement<CheckBoxModel>
{
    public CheckBox(AutomationElement element) 
        : base(element, ControlType.CheckBox)
    {
    }

    public bool? IsChecked
    {
        get => Element.GetToggleState();
        set => Element.TrySetToggleState(value);
    }
}