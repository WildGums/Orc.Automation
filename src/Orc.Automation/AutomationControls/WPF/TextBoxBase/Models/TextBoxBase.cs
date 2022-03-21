namespace Orc.Automation
{
    using System.Windows.Controls;
    using System.Windows.Media;

    [ActiveAutomationModel]
    public class TextBoxBase : ControlModel
    {
        public TextBoxBase(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        [ActiveAutomationProperty(nameof(TextBoxBase.IsReadOnly))]
        public bool IsTextBoxReadOnly { get; set; }
        public bool AcceptsReturn { get; set; }
        public bool AcceptsTab { get; set; }
        public bool AutoWordSelection { get; set; }
        public bool CanRedo { get; set; }
        public bool CanUndo { get; set; }
        public SolidColorBrush CaretBrush { get; set; }
        public double ExtentHeight { get; set; }
        public double HorizontalOffset { get; set; }
        public double ExtentWidth { get; set; }
        public ScrollBarVisibility HorizontalScrollBarVisibility { get; set; }
        public ScrollBarVisibility VerticalScrollBarVisibility { get; set; }
        public bool IsInactiveSelectionHighlightEnabled { get; set; }
        public bool IsReadOnlyCaretVisible { get; set; }
        public bool IsSelectionActive => _accessor.GetValue<bool>();
        public SolidColorBrush SelectionBrush { get; set; }
        public SolidColorBrush SelectionTextBrush { get; set; }
        public double SelectionOpacity { get; set; }
        public double VerticalOffset { get; set; }
        public int UndoLimit { get; set; }
        public double ViewportHeight { get; set; }
        public double ViewportWidth { get; set; }
    }
}
