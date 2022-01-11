namespace Orc.Automation.Tests.StyleAsserters;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Media;
using Catel;
using Controls;
using NUnit.Framework;
using Theming;

public enum ColorType
{
    Border,
    Background,
    Foreground
}

public enum ColorState
{
    Default,
    Disabled,
    Checked,
    CheckedMouseOver,
    MouseOver,
    Focus,
    Pressed,
    Highlighted,
    SelectionActive,
    SelectionInactive
}

public abstract class ThemeColorAssert<TAssert, TElement>
    where TAssert : ThemeColorAssert<TAssert, TElement>, new()
    where TElement : FrameworkElement
{
    public static void VerifyThemeColors(TElement element) => new TAssert().VerifyColors(element);

    protected virtual void VerifyColors(TElement element)
    {
        foreach (ColorType colorType in ColorTypes)
        {
            foreach (ColorState colorState in ColorStates)
            {
                SetControlInState(element, colorState);

                Wait.UntilResponsive();

                Assert.That(GetColor(element, colorType), Is.EqualTo(GetExpectedColor(element, colorType, colorState)));
            }
        }
    }

    protected virtual IList<ColorType> ColorTypes { get; } = Enum.GetValues(typeof(ColorType)).OfType<ColorType>().ToList();
    protected virtual IList<ColorState> ColorStates { get; } = Enum.GetValues(typeof(ColorState)).OfType<ColorState>().ToList();

    protected abstract void SetControlInState(FrameworkElement element, ColorState colorState);

    protected abstract Color? GetColor(FrameworkElement element, ColorType colorType);

    protected virtual Color? GetExpectedColor(FrameworkElement element, ColorType colorType, ColorState colorState)
    {
        return GetColorFromResources(element, colorType, colorState);
    }

    protected virtual Color? GetColorFromResources(FrameworkElement element, ColorType colorType, ColorState colorState)
    {
        var resourceName = $"Orc.Brushes.Control.{colorType}.{colorState}";

        return GetColorFromResources(element, resourceName);
    }

    private static Color? GetColorFromResources(FrameworkElement element, string brushName)
    {
        return (element.TryFindResource(brushName) as SolidColorBrush)?.Color;
    }
}

public class ButtonThemeColorAssert : ThemeColorAssert<ButtonThemeColorAssert, Button>
{
    private ButtonThemeMap _buttonThemeMap;

    protected override void VerifyColors(Button element)
    {
        using (new DisposableToken<ButtonThemeColorAssert>(this, x => _buttonThemeMap = element.Map<ButtonThemeMap>(), x => _buttonThemeMap = null))
        {
            base.VerifyColors(element);
        }
    }

    protected override IList<ColorType> ColorTypes => new [] { ColorType.Border, ColorType.Background};
    protected override IList<ColorState> ColorStates => new[] { ColorState.Default, ColorState.MouseOver };
    protected override void SetControlInState(FrameworkElement element, ColorState colorState)
    {
        switch (colorState)
        {
            case ColorState.Default:
                element.MouseOut();
                return;

            case ColorState.MouseOver:
                element.MouseHover();
                return;
        }
    }

    protected override Color? GetColor(FrameworkElement element, ColorType colorType)
    {
        return colorType switch
        {
            ColorType.Border => _buttonThemeMap.Chrome.BorderBrush.Color,
            ColorType.Background => _buttonThemeMap.Chrome.Background.Color,
            _ => throw new ArgumentOutOfRangeException(nameof(colorType), colorType, null)
        };
    }
}

public class ThemeAssert
{
    public const string ControlDefaultBackgroundBrushResourceName = "Orc.Brushes.Control.Default.Background";
    public const string ControlDefaultBorderBrushResourceName = "Orc.Brushes.Control.Default.Border";

    public const string ControlDefaultMouseOverBrushResourceName = "Orc.Brushes.Control.MouseOver.Background";
    public const string ControlDefaultForegroundResourceName = "Orc.Brushes.Control.MouseOver.Foreground";

    //public static void VerifyDefaultBackground(FrameworkElement element)
    //{
    //    Assert.That(element.Background.Color, Is.EqualTo(GetColorFromNamedBrush(element, ControlDefaultBorderBrushResourceName)));
    //}

    //public static void VerifyMouseOverBackground(FrameworkElement element)
    //{
    //    VerityBackground(element, ControlDefaultBackgroundBrushResourceName);
    //}

    //public static void VerifyBackground(FrameworkElement element, string resourceName)
    //{
    //    Assert.That(element.Background.Color, Is.EqualTo(GetColorFromNamedBrush(element, resourceName)));
    //}

    //public static void VerifyColor(FrameworkElement element, ColorType colorType, ColorState colorState)
    //{
    //    if(colorType)
    //}

    //private static Color? GetColorFromNamedBrush(FrameworkElement element, string brushName)
    //{
    //    return (element.TryFindResource(brushName) as SolidColorBrush)?.Color;
    //}

    

   // public static SolidColorBrush GetDefaultBackground(FrameworkElement element)
    //    => element.TryFindResource(ControlDefaultBackgroundBrushResourceName) as SolidColorBrush;
    
  //  public static SolidColorBrush GetBackground(FrameworkElement element)
    //    => element.TryFindResource(ControlDefaultForegroundResourceName) as SolidColorBrush;
}
