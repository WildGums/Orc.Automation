namespace Orc.Automation
{
    [ActiveAutomationModel]
    public class HeaderedContentControlModel : ContentControlModel
    {
        public HeaderedContentControlModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public object Header { get; set; }
        public bool HasHeader { get; set; }
        public string HeaderStringFormat { get; set; }
    }
}
