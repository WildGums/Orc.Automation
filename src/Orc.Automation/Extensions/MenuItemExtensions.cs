namespace Orc.Automation;

using System;
using System.Linq;
using Controls;

public static class MenuItemExtensions
{
    public static MenuItem Select(this MenuItem menuItem, string header)
    {
        return Select(menuItem, x => Equals(x.Element.TryGetDisplayText(), header));
    }

    public static MenuItem Select(this MenuItem menuItem, Func<MenuItem, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(menuItem);
        var items = menuItem.Items;

        var childMenuItem = items?.FirstOrDefault(predicate);
        childMenuItem?.Click();

        return childMenuItem;
    }
}
