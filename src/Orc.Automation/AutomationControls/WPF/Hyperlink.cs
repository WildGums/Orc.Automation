namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Hyperlink))]
    public class Hyperlink : FrameworkElement
    {
        public Hyperlink(AutomationElement element)
            : base(element, ControlType.Hyperlink)
        {
        }

        public void Invoke()
        {
            Element.Invoke();
        }
    }
}
