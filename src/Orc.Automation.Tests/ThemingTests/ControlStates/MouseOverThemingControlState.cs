namespace Orc.Automation.Tests;

using Controls;

public class MouseOverThemingControlState<TElement> : ThemingControlStateBase<TElement>
    where TElement : FrameworkElement
{
    public override void SetControlInState(TElement element)
    {
        var automationElement = element.Element;

        automationElement.MouseHover();
    }
}