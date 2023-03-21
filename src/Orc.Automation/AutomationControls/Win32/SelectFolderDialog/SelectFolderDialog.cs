namespace Orc.Automation.Controls;

using System.Windows.Automation;

[Control(ClassName = Win32AutomationDialogsClassNames.SelectFolderDialogClassName)]
public class SelectFolderDialog : Window<WindowModel, SelectFolderDialogMap>
{
    public static void WaitAccept(string folderPath)
    {
        var openFileDialog = Wait();

        openFileDialog.FolderPath = folderPath;

        openFileDialog.Accept();
    }

    public static SelectFolderDialog Wait() => Window.WaitForWindow<SelectFolderDialog>();
        
    public SelectFolderDialog(AutomationElement element)
        : base(element)
    {
    }

    public string? FolderPath
    {
        get => Map.FolderEdit?.Text;
        set => SetMapValue(Map.FolderEdit, nameof(Edit.Text), value);
    }

    public void Accept()
    {
        Map.SelectFolderButton?.Click();
    }

    public void Cancel()
    {
        Map.CancelButton?.Click();
    }
}