namespace Orc.Automation.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;

    [AutomatedControl(ClassName = Win32AutomationDialogsClassNames.SaveFileDialogClassName)]
    public class SaveFileDialog : Window<WindowModel, SaveFileDialogMap>
    {
        public static void WaitAccept(string fileName)
        {
            var dialog = Wait();

            dialog.FilePath = fileName;
            
            dialog.Accept();
        }
        public static SaveFileDialog Wait() => Window.WaitForWindow<SaveFileDialog>();

        public SaveFileDialog(AutomationElement element) 
            : base(element)
        {
        }

        public string FilePath
        {
            get => Map.FileNameComboBox.Text;
            set => Map.FileNameComboBox.Text = value;
        }

        public string FileType
        {
            get => Map.FileTypeComboBox.Text;
            set => Map.FileTypeComboBox.Text = value;
        }

        public IReadOnlyList<string> AvailableFileTypes
        {
            get => Map.FileTypeComboBox.Items.Select(x => x.Current.Name).ToList();
        }

        public void Accept()
        {
            Map.SaveButton.Click();
        }

        public void Cancel()
        {
            Map.CancelButton.Click();
        }
    }
}
