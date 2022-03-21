namespace Orc.Automation.Tests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Controls;
using NUnit.Framework;

public abstract class ThemeAssertBase<TAssert, TElement> 
    where TAssert : ThemeAssertBase<TAssert, TElement>, new()
    where TElement : AutomationControl
{
    public static ThemeControlStates<TElement> States => new();

    public static void VerifyTheme(TElement element) => new TAssert().VerifyThemeColors(element);

    protected virtual void VerifyThemeColors(TElement element)
    {
        foreach (IThemingControlState<TElement> colorState in ThemingStates ?? Enumerable.Empty<IThemingControlState<TElement>>())
        {
            foreach (ColorType colorType in ColorTypes)
            {
                colorState.SetControlInState(element);

                Wait.UntilResponsive();

                var color = GetColor(element, colorType);
                var expectedColor = colorState.GetColor(element, colorType);

                Assert.That(color, Is.EqualTo(expectedColor));
            }
        }
    }

    protected virtual IList<IThemingControlState<TElement>> ThemingStates { get; } = new List<IThemingControlState<TElement>>();

    protected virtual IList<ColorType> ColorTypes { get; } = Enum.GetValues(typeof(ColorType)).OfType<ColorType>().ToList();

    protected abstract Color? GetColor(TElement element, ColorType colorType);
}
