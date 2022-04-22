namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    public class SaveFileDialogMap : AutomationBase
    {
        public SaveFileDialogMap(AutomationElement element)
            : base(element)
        {
        }

        public Edit SearchEdit => By.Id("SearchEditBox").One<Edit>();
        public ComboBox FileNameComboBox => By.Id("FileNameControlHost").One<ComboBox>();
        public ComboBox FileTypeComboBox => By.Id("FileTypeControlHost").One<ComboBox>();
        public Button SaveButton => By.Name("Save").One<Button>();
        public Button CancelButton => By.Name("Cancel").One<Button>();
    }
}
