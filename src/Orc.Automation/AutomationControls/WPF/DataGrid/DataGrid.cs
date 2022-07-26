namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using Controls;

    [Control(ControlTypeName = nameof(ControlType.DataGrid))]
    public class DataGrid : FrameworkElement<DataGridModel>
    {
        public DataGrid(AutomationElement element) 
            : base(element)
        {
        }

        public int RowCount => Element.GetRowCount();
        public int ColumnCount => Element.GetColumnCount();

        public IReadOnlyList<DataItem> Rows => By.Many<DataItem>();

        public IReadOnlyList<HeaderItem> ColumnHeaders =>
            Element.GetColumnHeaders()?.Select(x => x.As<HeaderItem>()).ToList() ?? new List<HeaderItem>();

        public IReadOnlyList<HeaderItem> RowHeaders =>
            Element.GetRowHeaders()?.Select(x => x.As<HeaderItem>()).ToList() ?? new List<HeaderItem>();

        public DataGridCell this[int columnIndex, int rowIndex]
        {
            get => Element.GetTableItem(rowIndex, columnIndex).As<DataGridCell>();
        }
    }
}
