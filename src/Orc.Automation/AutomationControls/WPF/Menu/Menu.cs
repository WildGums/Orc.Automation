namespace Orc.Automation.Controls
{
    using System.Collections.Generic;
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Menu))]
    public class Menu : FrameworkElement<MenuModel>
    {
        public Menu(AutomationElement element)
            : base(element, ControlType.Menu)
        {
        }

        public IList<MenuItem> Items => By.Many<MenuItem>();
    }
}
