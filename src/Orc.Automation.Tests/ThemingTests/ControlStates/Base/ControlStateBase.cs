namespace Orc.Automation.Tests
{
    using Controls;

    public abstract class ControlStateBase<TElement> : IControlState<TElement>
        where TElement : FrameworkElement
    {
        public abstract void SetControlInState(TElement element);
    }
}
