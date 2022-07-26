namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    [Control(ControlTypeName = nameof(ControlType.Button))]
    public class Button : FrameworkElement<ButtonModel>
    {
        public Button(AutomationElement element) 
            : base(element, ControlType.Button)
        {
        }

        public string Content => Element.Current.Name;

        public bool Click()
        {
            return Element.TryInvoke();
        }
    }
}
