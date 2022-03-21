namespace Orc.Automation
{
    using System.Windows.Automation;
    using Controls;

    [AutomatedControl(ClassName = "DataGridCell")]
    public class DataGridCell : FrameworkElement<DataGridCellModel>
    {
        public DataGridCell(AutomationElement element) 
            : base(element)
        {
        }

        public string Text => Element.TryGetDisplayText();
    }
}
