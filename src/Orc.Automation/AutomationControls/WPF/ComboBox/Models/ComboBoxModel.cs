namespace Orc.Automation
{
    [ActiveAutomationModel]
    public class ComboBoxModel : SelectorModel
    {
        public ComboBoxModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public string Text { get; set; }

        [ActiveAutomationProperty(OriginalName = nameof(System.Windows.Controls.ComboBox.IsReadOnly))]
        public bool IsComboBoxReadOnly { get; set; }
        public bool IsDropDownOpen { get; set; }
        public bool IsEditable { get; set; }
        public double MaxDropDownHeight { get; set; }
        public bool StaysOpenOnEdit { get; set; }
        public bool ShouldPreserveUserEnteredPrefix { get; set; }
        public object SelectionBoxItem { get; set; }
        public string SelectionBoxItemStringFormat { get; set; }
        public bool IsSelectionBoxHighlighted => _accessor.GetValue<bool>();
    }
}
