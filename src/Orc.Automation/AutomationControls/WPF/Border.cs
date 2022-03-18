namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [Raw]
    [AutomatedControl(Class = typeof(System.Windows.Controls.Border))]
    public class Border : FrameworkElement
    {
        public Border(AutomationElement element)
            : base(element)
        {
        }
    }
}
