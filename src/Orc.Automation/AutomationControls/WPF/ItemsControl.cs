namespace Orc.Automation
{
    using System.Windows.Automation;
    using Controls;

    [AutomatedControl(Class = typeof(System.Windows.Controls.ItemsControl))]
    public class ItemsControl : FrameworkElement<ItemsControlModel>
    {
        public ItemsControl(AutomationElement element)
            : base(element)
        {
        }
    }
}
