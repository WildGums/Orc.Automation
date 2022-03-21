namespace Orc.Automation
{
    [ActiveAutomationModel]
    public class MenuModel : MenuBaseModel
    {
        public MenuModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public bool IsMainMenu { get; set; }
    }
}
