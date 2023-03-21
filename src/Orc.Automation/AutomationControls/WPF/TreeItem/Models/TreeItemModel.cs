namespace Orc.Automation;

using System.Windows.Controls;

[ActiveAutomationModel]
public class TreeItemModel : HeaderedContentControlModel
{
    public TreeItemModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public bool IsSelected { get; set; }
    public Dock TabStripPlacement => _accessor.GetValue<Dock>();
}