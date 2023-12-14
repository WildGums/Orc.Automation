namespace Orc.Automation;

using System;
using System.Windows.Automation;
using System.Windows.Input;
using Tests;

public static partial class AutomationElementExtensions
{
    [UserInteraction]
    public static void MouseClick(this AutomationElement element, MouseButton mouseButton = MouseButton.Left)
    {
        ArgumentNullException.ThrowIfNull(element);
            
        element.MouseHover();
        MouseInput.Click(mouseButton);
    }

    [UserInteraction]
    public static void MouseHover(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        var rect = element.Current.BoundingRectangle;

        MouseInput.MoveTo(rect.GetClickablePoint());
    }

    [UserInteraction]
    public static void MouseOut(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        var rect = element.Current.BoundingRectangle;

        MouseInput.MoveTo(rect.GetPointOut());
    }
}
