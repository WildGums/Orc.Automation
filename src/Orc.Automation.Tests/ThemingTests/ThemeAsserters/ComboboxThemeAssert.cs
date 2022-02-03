namespace Orc.Automation.Tests.ThemingTests.ThemeAsserters
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Media;
    using Controls;

    public class ComboboxThemeAssert : MappedThemeAssertBase<ComboboxThemeAssert, ComboBox, ComboboxThemeMap>
    {
        protected override IList<IThemingControlState<ComboBox>> ThemingStates { get; } = new List<IThemingControlState<ComboBox>>()
        {
            States.Default,
            States.MouseOver
        };

        protected override IList<ColorType> ColorTypes => new[]
        {
            ColorType.Border,
            ColorType.Background
        };

        protected override Color? GetColor(ComboBox element, ColorType colorType)
        {
            return colorType switch
            {
                ColorType.Border => _map.Chrome.BorderBrush.Color,
                ColorType.Background => _map.Chrome.Background.Color,
                _ => throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null)
            };
        }
    }
}
