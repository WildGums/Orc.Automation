﻿namespace Orc.Automation;

using System;
using System.Linq;
using Controls;

public static class MenuExtensions
{
    public static MenuItem Select(this Menu menu, Func<MenuItem, bool> predicate)
    {
        var items = menu.Items;
        var currentMenuItem = items.FirstOrDefault(predicate);
        currentMenuItem.Click();

        return currentMenuItem;
    }

    public static void Select(this Menu menu, params string[] headers)
    {
        ArgumentNullException.ThrowIfNull(menu);

        if(!headers.Any())
        {
            return;
        }    

        var items = menu.Items;
        var currentMenuItem = items?.FirstOrDefault(x => Equals(x.Header, headers[0]));
        if(currentMenuItem is null)
        {
            return;
        }

        currentMenuItem.Click();
        Wait.UntilResponsive(200);

        foreach (var header in headers.Skip(1))
        {
            var nextMenuItem = currentMenuItem.Items?.ToList().FirstOrDefault(x => Equals(x.Element.TryGetDisplayText(), header));

            currentMenuItem = nextMenuItem;
            if (currentMenuItem is null)
            {
                return;
            }

            currentMenuItem.Click();
            Wait.UntilResponsive();
        }
    }
}
