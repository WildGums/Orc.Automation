namespace Orc.Automation.Tests;

using Controls;

public class SelectionInactiveThemingControlState<TElement> : ThemingControlStateBase<TElement>
    where TElement : FrameworkElement
{
    public override void SetControlInState(TElement element)
    {
        var automationElement = element.Element;

        automationElement.TrySetSelection(true);
        automationElement.MouseOut();
    }
}