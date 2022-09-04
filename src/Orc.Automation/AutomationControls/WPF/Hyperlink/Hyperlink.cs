namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [Control(ControlTypeName = nameof(ControlType.Hyperlink))]
    public class Hyperlink : FrameworkElement<HyperlinkModel>
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
