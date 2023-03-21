namespace Orc.Automation;

using System.Windows.Automation;
using Controls;

[Control(ClassName = "DataGridCell")]
public class DataGridCell : FrameworkElement<DataGridCellModel>
{
    public DataGridCell(AutomationElement element) 
        : base(element)
    {
    }

    public string Text => Element.TryGetDisplayText();
}