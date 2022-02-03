namespace Orc.Automation.Tests;

using System;
using System.Collections.Generic;
using System.Windows.Media;
using Controls;

public class ButtonThemeAssert : MappedThemeAssertBase<ButtonThemeAssert, Button, ButtonThemeMap>
{
    protected override IList<IThemingControlState<Button>> ThemingStates { get; } = new List<IThemingControlState<Button>>()
    {
        States.Default,
        States.MouseOver
    };

    protected override IList<ColorType> ColorTypes => new[]
    {
        ColorType.Border,
        ColorType.Background
    };

    protected override Color? GetColor(Button element, ColorType colorType)
    {
        return colorType switch
        {
            ColorType.Border => _map.Chrome.BorderBrush.Color,
            ColorType.Background => _map.Chrome.Background.Color,
            _ => throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null)
        };
    }
}
