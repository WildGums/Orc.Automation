namespace Orc.Automation
{
    using Controls;

    public class PopupWindow
    {
        public static TView? Find<TView>(string? id = null, string? name = null)
            where TView : AutomationControl
        {
            var mainWindow = Window.MainWindow;
            return mainWindow.Find<TView>(id, name);
        }
    }
}
