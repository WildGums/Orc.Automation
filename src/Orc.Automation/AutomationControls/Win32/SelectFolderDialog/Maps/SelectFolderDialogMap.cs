namespace Orc.Automation.Controls
{
    using System.Windows.Automation;

    public class SelectFolderDialogMap : AutomationBase
    {
        public SelectFolderDialogMap(AutomationElement element) 
            : base(element)
        {
        }

        public Button SelectFolderButton => By.Name("Select Folder").One<Button>();
        public Button CancelButton => By.Name("Cancel").One<Button>();
        public Edit FolderEdit => By.Name("Folder:").One<Edit>();
    }
}
