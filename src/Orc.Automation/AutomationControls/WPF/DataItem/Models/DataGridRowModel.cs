namespace Orc.Automation
{
    using System.Windows;

    [ActiveAutomationModel]
    public class DataGridRowModel : FrameworkElementModel
    {
        public DataGridRowModel(AutomationElementAccessor accessor)
            : base(accessor)
        {
        }

        public object Header { get; set; }
        public bool IsSelected { get; set; }
        public bool IsNewItem => _accessor.GetValue<bool>();
        public Visibility DetailsVisibility { get; set; }
        public bool AlternationIndex => _accessor.GetValue<bool>();
        public bool IsEditing => _accessor.GetValue<bool>();
    }
}
