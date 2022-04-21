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

        public string Header => Element.TryGetDisplayText();

        public IList<MenuItem> Items => By.Many<MenuItem>();
            
        public void Click()
        {
            Element.TryClick();
        }
    }
}
