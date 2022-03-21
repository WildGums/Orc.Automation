namespace Orc.Automation.Tests;

using Controls;

public class DefaultThemingControlState<TElement> : ThemingControlStateBase<TElement>
    where TElement : AutomationControl
{
    public override void SetControlInState(TElement element)
    {
        var automationElement = element.Element;

        automationElement.TrySetSelection(false);
        automationElement.MouseOut();
    }
}
