namespace Orc.Automation;

using System.Windows.Controls;
using System.Windows.Input;

[ActiveAutomationModel]
public class MenuItemModel : FrameworkElementModel
{
    public MenuItemModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }

    public ICommand Command { get; set; }
    public object CommandParameter { get; set; }
    public string InputGestureText { get; set; }
    public bool StaysOpenOnClick { get; set; }
    public bool IsSubmenuOpen { get; set; }
    public bool IsCheckable { get; set; }
    public bool UsesItemContainerTemplate { get; set; }
    public bool IsHighlighted => _accessor.GetValue<bool>();
    public MenuItemRole Role => _accessor.GetValue<MenuItemRole>();
    public bool IsSuspendingPopupAnimation => _accessor.GetValue<bool>();
    public bool IsChecked { get; set; }
    public bool IsPressed => _accessor.GetValue<bool>();
}