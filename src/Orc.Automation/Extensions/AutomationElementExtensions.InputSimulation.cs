namespace Orc.Automation;

using System;
using System.Windows.Automation;
using System.Windows.Input;
using Catel;

public partial class AutomationElementExtensions
{
    public static void MouseClick(this AutomationElement element, MouseButton mouseButton = MouseButton.Left)
    {
        ArgumentNullException.ThrowIfNull(element);
            
        element.MouseHover();
        MouseInput.Click(mouseButton);
    }

    public static void MouseHover(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        var rect = element.Current.BoundingRectangle;

        MouseInput.MoveTo(rect.GetClickablePoint());
    }

    public static void MouseOut(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        var rect = element.Current.BoundingRectangle;

        MouseInput.MoveTo(rect.GetPointOut());
    }
}
