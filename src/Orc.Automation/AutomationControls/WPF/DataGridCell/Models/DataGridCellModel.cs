namespace Orc.Automation
{
    [ActiveAutomationModel]
    public class DataGridCellModel : ContentControlModel
    {
        public DataGridCellModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public bool IsEditing { get; set; }
        public bool IsSelected { get; set; }
    }
}
