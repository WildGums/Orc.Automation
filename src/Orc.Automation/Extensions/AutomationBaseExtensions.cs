namespace Orc.Automation;

using System;

public static class AutomationBaseExtensions
{
    public static bool IsVisible(this AutomationBase element)
    {
        ArgumentNullException.ThrowIfNull(element);

        //Automation can't find element if it's not visible, so no checks for null
        return element is not null && element.Element.IsVisible();
    }
}