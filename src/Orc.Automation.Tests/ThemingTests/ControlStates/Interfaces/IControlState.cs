namespace Orc.Automation.Tests
{
    public interface IControlState<in TElement>
        where TElement : AutomationControl
    {
        void SetControlInState(TElement element);
    }
}
