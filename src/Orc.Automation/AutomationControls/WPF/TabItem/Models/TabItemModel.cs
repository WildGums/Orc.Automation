namespace Orc.Automation;

using System.Windows.Controls;

[ActiveAutomationModel]
public class TabItemModel : HeaderedContentControlModel
{
    public TabItemModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public bool IsSelected { get; set; }
    public Dock TabStripPlacement { get; set; }
}