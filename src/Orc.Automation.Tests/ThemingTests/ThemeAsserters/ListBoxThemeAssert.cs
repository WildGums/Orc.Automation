namespace Orc.Automation.Tests
{
    using System.Windows.Media;
    using Controls;

    public class ListBoxThemeAssert : ThemeAssertBase<ListBoxThemeAssert, List>
    {
        protected override void VerifyThemeColors(List element)
        {
            base.VerifyThemeColors(element);
            
            var items = element.Items;
            foreach (var listItem in items)
            {
                ListBoxItemThemeAssert.VerifyTheme(listItem);
            }
        }

        protected override Color? GetColor(List element, ColorType colorType)
        {
            return colorType switch
            {
                ColorType.Border => element.Current.BorderBrush?.Color,
                ColorType.Background => element.Current.Background?.Color,
                _ => null
            };
        }
    }
}
