namespace Orc.Automation.Tests
{
    using System.Windows.Automation;
    using Controls;

    public class ListBoxItemThemeMap : AutomationBase
    {
        public ListBoxItemThemeMap(AutomationElement element)
            : base(element)
        {
        }

        public Border Bd => By.Name("border").Part<Border>();
    }
}
