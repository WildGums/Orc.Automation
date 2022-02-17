namespace Orc.Automation.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;

    public class OpenFileDialogMap : AutomationBase
    {
        public OpenFileDialogMap(AutomationElement element)
            : base(element)
        {
        }

        public Button AcceptButton => By.Id("1").One<Button>();
        public Button CancelButton => By.Id("2").One<Button>();
        public ComboBox FilePathCombobox => By.Id("1148").One<ComboBox>();
        public ComboBox FiltersCombobox => By.Id("1136").One<ComboBox>();
    }

    public class OpenFileDialog : AutomationBase
    {
        private readonly OpenFileDialogMap _map;

        public OpenFileDialog(AutomationElement element)
            : base(element)
        {
            _map = new OpenFileDialogMap(element);
        }

        public string FilePath
        {
            get => _map.FilePathCombobox.Text;
            set => _map.FilePathCombobox.Text = value;
        }

        public List<string> Filters
        {
            get => _map.FiltersCombobox.Items.Select(x => x.Current.Name).ToList();
        }

        public void Accept()
        {
            _map.AcceptButton.Click();
        }

        public void Cancel()
        {
            _map.CancelButton.Click();
        }
    }
}
