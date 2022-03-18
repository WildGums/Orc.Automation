namespace Orc.Automation.Controls
{
    using System.Windows.Automation;
    using System.Windows.Input;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Button))]
    public sealed class Button : FrameworkElement
    {
        public Button(AutomationElement element) 
            : base(element, ControlType.Button)
        {

        }

        public ICommand Command
        {
            get => Access.GetValue<ICommand>();
            set => Access.SetValue(value);
        }

        public object CommandParameter
        {
            get => Access.GetValue<object>();
            set => Access.SetValue(value);
        }

        public string Content => Element.Current.Name;

        public bool Click()
        {
            return Element.TryInvoke();
        }
    }
}
