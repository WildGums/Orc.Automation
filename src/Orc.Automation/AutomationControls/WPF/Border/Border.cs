namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [Raw]
    [Control(Class = typeof(System.Windows.Controls.Border))]
    public class Border : FrameworkElement<BorderModel>
    {
        public Border(AutomationElement element)
            : base(element)
        {
        }
    }
}
