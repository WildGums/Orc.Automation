namespace Orc.Automation.Controls;

using System.Windows.Automation;

/// <summary>
/// Toggle button Automation Element wrapper
/// </summary>
[Control(ControlTypeName = nameof(ControlType.Button))]
public class ToggleButton : FrameworkElement<ToggleButtonModel>
{
    public ToggleButton(AutomationElement element)
        : base(element, ControlType.Button)
    {
    }

    public string Content => Element.TryGetDisplayText();

    /// <summary>
    /// Get Is button is toggled
    /// </summary>
    public bool IsToggled
    {
        get => Element.GetToggleState() == true;
        set => Element.TrySetToggleState(value);
    }

    /// <summary>
    /// Toggles button
    /// </summary>
    /// <returns>The button state after toggle</returns>
    /// <exception cref="AutomationException">if toggle pattern is not available</exception>
    /// <remarks>Note: In some WPF scenarios binded command might not be executed. In this case try use <see cref="AutomationControlExtensions.MouseClick"/></remarks>
    public bool? Toggle() => Element.Toggle();
}