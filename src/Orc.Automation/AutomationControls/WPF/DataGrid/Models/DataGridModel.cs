namespace Orc.Automation
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;

    [ActiveAutomationModel]
    public class DataGridModel : ItemsControlModel
    {
        public DataGridModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        [ActiveAutomationProperty(OriginalName = nameof(System.Windows.Controls.DataGrid.IsReadOnly))]
        public bool IsDataGridReadOnly { get; set; }
        public ScrollBarVisibility VerticalScrollBarVisibility { get; set; }
        public ScrollBarVisibility HorizontalScrollBarVisibility { get; set; }
        public SolidColorBrush AlternatingRowBackground { get; set; }
        public bool AreRowDetailsFrozen { get; set; }
        public bool AutoGenerateColumns { get; set; }
        public bool CanUserAddRows { get; set; }
        public bool CanUserDeleteRows { get; set; }
        public bool CanUserReorderColumns { get; set; }
        public bool CanUserResizeColumns { get; set; }
        public bool CanUserResizeRows { get; set; }
        public bool EnableColumnVirtualization { get; set; }
        public int FrozenColumnCount { get; set; }
        public DataGridLength ColumnWidth { get; set; }
        public double CellsPanelHorizontalOffset => _accessor.GetValue<double>();
        public DataGridHeadersVisibility HeadersVisibility { get; set; }
        public double MaxColumnWidth { get; set; }
        public double MinColumnWidth { get; set; }
        public double MinRowHeight { get; set; }
        public Thickness NewItemMargin { get; set; }
        public SolidColorBrush RowBackground { get; set; }
        public double NonFrozenColumnsViewportHorizontalOffset => _accessor.GetValue<double>();
        public double RowHeaderActualWidth => _accessor.GetValue<double>();
        public double RowHeaderWidth { get; set; }
        public double RowHeight { get; set; }
        public DataGridSelectionUnit SelectionUnit { get; set; }
        public SolidColorBrush VerticalGridLinesBrush { get; set; }

        [ActiveAutomationProperty]
        public double ColumnHeaderHeight { get; set; }
    }
}
