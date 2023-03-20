namespace Orc.Automation.Controls;

using System.Collections.Generic;
using System.Windows.Automation;

[Control(ControlTypeName = nameof(ControlType.Menu))]
public class Menu : FrameworkElement<MenuModel>
{
    public static Menu WaitForContextMenu()
    {
        var popup = Window.MainWindow.Find(className: "Popup", controlType: ControlType.Window);
        var contextMenu = popup?.Find<Menu>() ?? Window.MainWindow.Find<Menu>();

        return contextMenu;
    }

    public Menu(AutomationElement element)
        : base(element, ControlType.Menu)
    {
    }

    public IList<MenuItem> Items => By.Many<MenuItem>();
}