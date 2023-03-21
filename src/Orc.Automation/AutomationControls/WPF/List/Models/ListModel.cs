namespace Orc.Automation;

using System.Collections;
using System.Windows.Controls;

[ActiveAutomationModel]
public class ListModel : SelectorModel
{
    public ListModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public SelectionMode SelectionMode { get; set; }
    public IList SelectedItems { get; set; }
}