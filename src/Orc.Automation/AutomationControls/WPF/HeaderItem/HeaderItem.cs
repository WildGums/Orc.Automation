namespace Orc.Automation
{
    using System.Windows.Automation;
    using Controls;

    [AutomatedControl(ControlTypeName = nameof(ControlType.HeaderItem))]
    public class HeaderItem : FrameworkElement<HeaderItemModel>
    {
        public HeaderItem(AutomationElement element)
            : base(element)
        {
        }

        public string Header => Element.TryGetDisplayText();

        public void Click() => Element.TryInvoke();
    }
}
