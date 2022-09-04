namespace Orc.Automation
{
    using System.Windows.Controls;

    [ActiveAutomationModel]
    public class ComboBoxModel : SelectorModel
    {
        public ComboBoxModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public string Text { get; set; }

        [ActiveAutomationProperty(OriginalName = nameof(ComboBox.IsReadOnly))]
        public bool IsComboBoxReadOnly { get; set; }
        public bool IsDropDownOpen { get; set; }
        public bool IsEditable { get; set; }
        public double MaxDropDownHeight { get; set; }
        public bool StaysOpenOnEdit { get; set; }
        public bool ShouldPreserveUserEnteredPrefix { get; set; }
        public object SelectionBoxItem { get; set; }
        public string SelectionBoxItemStringFormat { get; set; }
        public bool IsSelectionBoxHighlighted => _accessor.GetValue<bool>();

        [ActiveAutomationProperty(OwnerType = typeof(VirtualizingPanel))]
        public bool IsContainerVirtualizable { get; set; }

        [ActiveAutomationProperty(OwnerType = typeof(VirtualizingPanel))]
        public bool IsVirtualizing { get; set; }
    }
}
