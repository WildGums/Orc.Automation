namespace Orc.Automation
{
    using System.Windows.Controls;
    using System.Windows.Input;

    [ActiveAutomationModel]
    public class ButtonBaseModel : ContentControlModel
    {
        public ButtonBaseModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public ICommand Command { get; set; }
        public ClickMode ClickMode { get; set; }
        public object CommandParameter { get; set; }
        public bool IsPressed { get; set; }
    }
}
