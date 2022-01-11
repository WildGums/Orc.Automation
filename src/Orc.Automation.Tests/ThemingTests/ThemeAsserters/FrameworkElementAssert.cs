namespace Orc.Automation.Tests;

using System.Windows.Media;
using Controls;
using NUnit.Framework;
using StyleAsserters;

//public class FrameworkElementAssert : ThemeAssert
//{
//    public virtual void Theme(FrameworkElement element)
//    {
//     //   BorderColor(element);
//     //   Background(element);
//    }

//    //private static void BorderColor(FrameworkElement element)
//    //{
//    //    var controlDefaultBorderBrush = GetDefaultBackground(element);

//    //    Assert.That(element.BorderBrush.Color, Is.EqualTo(controlDefaultBorderBrush?.Color));
//    //}

//    //private static void Background(FrameworkElement element)
//    //{
//    //    var controlDefaultBackground = element.TryFindResource(ControlDefaultBackgroundBrushResourceName) as SolidColorBrush;

//    //    Assert.That(element.Background.Color, Is.EqualTo(controlDefaultBackground?.Color));
//    //}
//}

public class FrameworkElementAssert<TAssert, TElement> : ThemeAssert
    where TAssert : FrameworkElementAssert<TAssert, TElement>, new()
    where TElement : FrameworkElement
{
    public static void VerifyTheme(TElement element) => new TAssert().Theme(element);

    public virtual void Theme(TElement element)
    {
     //   BorderColor(element);
     //   Background(element);
    }
}
