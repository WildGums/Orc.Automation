namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    public class OpenFileDialogMap : AutomationBase
    {
        public OpenFileDialogMap(AutomationElement element)
            : base(element)
        {
        }

        public Button? AcceptButton => By.Id("1").One<Button>();
        public Button? CancelButton => By.Id("2").One<Button>();
        public ComboBox? FilePathCombobox => By.Id("1148").One<ComboBox>();
        public ComboBox? FiltersCombobox => By.Id("1136").One<ComboBox>();
    }
}
