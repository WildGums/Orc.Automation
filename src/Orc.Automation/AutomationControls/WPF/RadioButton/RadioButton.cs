namespace Orc.Automation;

using System.Windows.Automation;
using Controls;

/// <summary>
/// Radio button Automation Element wrapper
/// </summary>
[Control(ControlTypeName = nameof(ControlType.RadioButton))]
public class RadioButton : FrameworkElement<RadioButtonModel>
{
    public RadioButton(AutomationElement element)
        : base(element, ControlType.RadioButton)
    {
    }

    /// <summary>
    /// Get Is button is selected
    /// </summary>
    public bool IsSelected
    {
        get => Element.GetIsSelected();
        set => Element.TrySetSelection(value);
    }

    public void Select() => Element.Select();
}
