namespace Orc.Automation;

[ActiveAutomationModel]
public class ListItemModel : FrameworkElementModel
{
    public ListItemModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }

    public bool IsSelected { get; set; }
}