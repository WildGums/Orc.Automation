namespace Orc.Automation.Tests;

using Controls;

public class CheckedMouseOverThemingControlState<TElement> : ThemingControlStateBase<TElement>
    where TElement : FrameworkElement
{
    public override void SetControlInState(TElement element)
    {
        var automationElement = element.Element;

        automationElement.TrySetToggleState(true);
        automationElement.MouseHover();
    }
}