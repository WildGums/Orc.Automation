namespace Orc.Automation.Tests
{
    using Controls;

    public interface IControlState<in TElement>
        where TElement : FrameworkElement
    {
        void SetControlInState(TElement element);
    }
}
