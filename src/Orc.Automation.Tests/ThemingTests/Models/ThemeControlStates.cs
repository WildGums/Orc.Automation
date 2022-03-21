namespace Orc.Automation.Tests;

using Controls;

public class ThemeControlStates<TElement>
    where TElement : AutomationControl
{
    public IThemingControlState<TElement> Default { get; } = new DefaultThemingControlState<TElement>();
    public IThemingControlState<TElement> MouseOver { get; } = new MouseOverThemingControlState<TElement>();
    public IThemingControlState<TElement> SelectionActive { get; } = new SelectionActiveThemingControlState<TElement>();
}
