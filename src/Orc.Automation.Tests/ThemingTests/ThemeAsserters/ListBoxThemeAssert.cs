namespace Orc.Automation.Tests
{
    using System;
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
                ColorType.Border => element.BorderBrush?.Color,
                ColorType.Background => element.Background?.Color,
                _ => null
            };
        }
    }
}
