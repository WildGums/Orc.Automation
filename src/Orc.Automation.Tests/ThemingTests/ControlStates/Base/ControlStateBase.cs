namespace Orc.Automation.Tests;

public abstract class ControlStateBase<TElement> : IControlState<TElement>
    where TElement : AutomationControl
{
    public abstract void SetControlInState(TElement element);
}