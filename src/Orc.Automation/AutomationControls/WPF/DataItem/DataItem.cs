namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Controls;

    [Control(ControlTypeName = nameof(ControlType.DataItem))]
    public class DataItem : FrameworkElement<DataGridRowModel>
    {
        public DataItem(AutomationElement element)
            : base(element)
        {
        }

        public IReadOnlyList<DataGridCell> Cells => By.Many<DataGridCell>();
    }
}
