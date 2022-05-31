namespace Orc.Automation.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;

    [AutomatedControl(ClassName = Win32AutomationDialogsClassNames.OpenFileDialogClassName)]
    public class OpenFileDialog : Window<WindowModel, OpenFileDialogMap>
    {
        public static void WaitAccept(string fileName)
        {
            var openFileDialog = Wait();
            openFileDialog.FilePath = fileName;

            openFileDialog.Accept();
        }

        public static OpenFileDialog Wait() => Window.WaitForWindow<OpenFileDialog>();


        public OpenFileDialog(AutomationElement element)
            : base(element)
        {
        }

        public string FilePath
        {
            get => Map.FilePathCombobox.Text;
            set => Map.FilePathCombobox.Text = value;
        }

        public List<string> Filters
        {
            get => Map.FiltersCombobox.Items.Select(x => x.Current.Name).ToList();
        }

        public void Accept()
        {
            Map.AcceptButton.Click();
        }

        public void Cancel()
        {
            Map.CancelButton.Click();
        }
    }
}
