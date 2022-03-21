namespace Orc.Automation
{
    [ActiveAutomationModel]
    public class ToggleButtonModel : ButtonBaseModel
    {
        public ToggleButtonModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public bool IsChecked { get; set; }
        public bool IsThreeState { get; set; }
    }
}
