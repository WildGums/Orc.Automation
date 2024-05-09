namespace Orc.Automation.Tests;

public class MouseOverThemingControlState<TElement> : ThemingControlStateBase<TElement>
    where TElement : AutomationControl
{
    public override void SetControlInState(TElement element)
    {
        var automationElement = element.Element;

        automationElement.MouseHover();
    }
}