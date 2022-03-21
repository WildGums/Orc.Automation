namespace Orc.Automation
{
    [ActiveAutomationModel]
    public class MenuBaseModel : ItemsControlModel
    {
        public MenuBaseModel(AutomationElementAccessor accessor)
            : base(accessor)
        {
        }

        public bool UsesItemContainerTemplate { get; set; }
    }
}
