namespace Orc.Automation.Tests.StyleAsserters
{
    using System.Linq;
    using System.Windows.Automation;
    using System.Windows.Media;
    using Catel.Windows;
    using Controls;
    using NUnit.Framework;

    public class ButtonThemeMap : AutomationBase
    {
        public ButtonThemeMap(AutomationElement button)
            : base(button)
        {
        }

        public FrameworkElement Chrome => By.Name().Part<Border>();
    }

    public class ThemeAssert
    {
        public const string ControlDefaultBackgroundBrushResourceName = "Orc.Brushes.Control.Default.Background";
        public const string ControlDefaultMouseOverBrushResourceName = "Orc.Brushes.Control.MouseOver.Background";
        public const string ControlDefaultBorderBrushResourceName = "Orc.Brushes.Control.Default.Border";
    }

    public class FrameworkElementAssert : ThemeAssert
    {
        public virtual void Theme(FrameworkElement element)
        {
            BorderColor(element);
            Background(element);
        }

        public static void BorderColor(FrameworkElement element)
        {
            var controlDefaultBorderBrush = element.TryFindResource(ControlDefaultBorderBrushResourceName) as SolidColorBrush;

            Assert.That(element.BorderBrush.Color, Is.EqualTo(controlDefaultBorderBrush?.Color));
        }

        public static void Background(FrameworkElement element)
        {
            var controlDefaultBackground = element.TryFindResource(ControlDefaultBackgroundBrushResourceName) as SolidColorBrush;

            Assert.That(element.Background.Color, Is.EqualTo(controlDefaultBackground?.Color));
        }
    }

    public class FrameworkElementAssert<TAssert, TElement> : FrameworkElementAssert
        where TAssert : FrameworkElementAssert, new()
        where TElement : FrameworkElement
    {
        public static void VerifyTheme(TElement element) => new TAssert().Theme(element);
    }

    public class ButtonThemeAssert : FrameworkElementAssert<ButtonThemeAssert, Button>
    {
        public override void Theme(FrameworkElement element)
        {
            base.Theme(element);

            MouseOverBackground(element);
        }

        private static void MouseOverBackground(FrameworkElement element)
        {
            var controlDefaultMouseOverBrush = element.TryFindResource(ControlDefaultMouseOverBrushResourceName) as SolidColorBrush;
            
            element.MouseHover();

            var selectionButtonMap = element.Map<ButtonThemeMap>();

            var chrome = selectionButtonMap.Chrome;

            Assert.That(chrome.Background.Color, Is.EqualTo(controlDefaultMouseOverBrush?.Color));
        }
    }
}
