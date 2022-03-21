namespace Orc.Automation
{
    [ActiveAutomationModel]
    public class TreeModel : ItemsControlModel
    {
        public TreeModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public object SelectedValuePath { get; set; }
        public object SelectedItem => _accessor.GetValue<object>();
        public object SelectedValue => _accessor.GetValue<object>();
    }
}
