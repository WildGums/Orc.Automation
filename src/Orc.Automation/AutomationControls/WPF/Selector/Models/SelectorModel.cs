namespace Orc.Automation;

[ActiveAutomationModel]
public class SelectorModel : ItemsControlModel
{
    public SelectorModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public object SelectedItem { get; set; }
    public int SelectedIndex { get; set; }
    public object SelectedValue { get; set; }
    public string SelectedValuePath { get; set; }
    public bool IsSynchronizedWithCurrentItem { get; set; }
}