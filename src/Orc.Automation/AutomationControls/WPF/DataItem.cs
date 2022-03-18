namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Windows.Automation;
    using Controls;

    [AutomatedControl(ControlTypeName = nameof(ControlType.DataItem))]
    public class DataItem : FrameworkElement
    {
        public DataItem(AutomationElement element)
            : base(element)
        {
        }

        public IReadOnlyList<DataGridCell> Cells => By.Many<DataGridCell>();
    }
}
