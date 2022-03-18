namespace Orc.Automation.Tests
{
    using System.Windows.Automation;
    using Controls;

    public class ComboboxThemeMap : AutomationBase
    {
        public ComboboxThemeMap(AutomationElement combobox)
            : base(combobox)
        {
        }

        public Border Chrome => By.Name().Part<Border>();
    }
}
