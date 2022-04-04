namespace Orc.Automation.Controls
{
    using System.Collections.Generic;
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.MenuItem))]
    public class MenuItem : FrameworkElement<MenuItemModel>
    {
        public MenuItem(AutomationElement element) 
            : base(element, ControlType.MenuItem)
        {
        }

        public IList<MenuItem> Items => By.Many<MenuItem>();
            
        public void Click()
        {
            Element.TryClick();
        }
    }
}
