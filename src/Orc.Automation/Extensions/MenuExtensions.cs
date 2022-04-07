namespace Orc.Automation
{
    using System.Linq;
    using System.Windows.Automation;
    using Catel;
    using Controls;

    public static class MenuExtensions
    {
        public static void Select(this Menu menu, string header)
        {
            Argument.IsNotNull(() => menu);

            var items = menu.Items;

            var menuItem = items?.FirstOrDefault(x => Equals(x.Element.TryGetDisplayText(), header));
            menuItem?.Click();
        }

        public static void Select(this Menu menu, params string[] headers)
        {
            Argument.IsNotNull(() => menu);

            if(!headers.Any())
            {
                return;
            }    

            var items = menu.Items;
            var currentMenuItem = items?.FirstOrDefault(x => Equals(x.Element.TryGetDisplayText(), headers[0]));
            if(currentMenuItem is  null)
            {
                return;
            }

            currentMenuItem.Click();
            Wait.UntilResponsive(200);

            foreach (var header in headers.Skip(1))
            {
                var childElements = currentMenuItem.Element.GetChildElements().ToList();
                var chileElements2 = currentMenuItem.Element.FindAll(controlType: ControlType.MenuItem, scope: TreeScope.Descendants).ToList();
                var items2 = currentMenuItem.Items;
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
}
