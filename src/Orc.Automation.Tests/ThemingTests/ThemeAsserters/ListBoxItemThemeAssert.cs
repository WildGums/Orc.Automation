namespace Orc.Automation.Tests;

using System.Collections.Generic;
using System.Windows.Media;
using Controls;

public class ListBoxItemThemeAssert : MappedThemeAssertBase<ListBoxItemThemeAssert, ListItem, ListBoxItemThemeMap>
{
    protected override IList<ColorType> ColorTypes => new[]
    {
        ColorType.Background
    };

    protected override IList<IThemingControlState<ListItem>> ThemingStates { get; }
        = new List<IThemingControlState<ListItem>>
        {
            States.Default,
            States.MouseOver,
            States.SelectionActive
        };

    protected override Color? GetColor(ListItem element, ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.Background:
                return _map.Bd.Current.Background?.Color;
        }

        return null;
    }
}