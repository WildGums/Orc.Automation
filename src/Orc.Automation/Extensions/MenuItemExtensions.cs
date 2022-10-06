namespace Orc.Automation
{
    using System;
    using System.Linq;
    using Catel;
    using Orc.Automation.Controls;

    public static class MenuItemExtensions
    {
        public static void Select(this MenuItem menuItem, string header)
        {
            ArgumentNullException.ThrowIfNull(menuItem);

            var items = menuItem.Items;

            var childMenuItem = items?.FirstOrDefault(x => Equals(x.Element.TryGetDisplayText(), header));
            childMenuItem?.Click();
        }
    }
}
