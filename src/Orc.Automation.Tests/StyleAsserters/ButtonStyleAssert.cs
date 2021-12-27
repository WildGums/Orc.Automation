namespace Orc.Automation.Tests.StyleAsserters
{
    using System.Linq;
    using System.Windows.Automation;
    using System.Windows.Controls;
    using System.Windows.Media;
    using Catel.Windows;
    using Controls;
    using NUnit.Framework;

    public class ChromeFinder : IPartFinder
    {
        public static readonly ChromeFinder Instance = new();

        public System.Windows.FrameworkElement Find(System.Windows.FrameworkElement element)
        {
            return element.GetChildren().FirstOrDefault() as Border;
        }
    }

    public class ButtonThemeMap : AutomationControl
    {
        public ButtonThemeMap(AutomationElement element)
            : base(element)
        {

        }

        public FrameworkElement Chrome => Part<FrameworkElement>(ChromeFinder.Instance);
    }


    public class ThemeAssert
    {
        public const string ControlDefaultBackgroundBrushResourceName = "Orc.Brushes.Control.Default.Background";
        public const string ControlDefaultMouseOverBrushResourceName = "Orc.Brushes.Control.MouseOver.Background";
        public const string ControlDefaultBorderBrushResourceName = "Orc.Brushes.Control.Default.Border";
    }

    public class FrameworkElementAssert : ThemeAssert
    {
        public static void BorderColor(FrameworkElement element)
        {
            var controlDefaultBorderBrush = element.TryFindResource(ControlDefaultBorderBrushResourceName) as SolidColorBrush;

            Assert.That(element.BorderBrush.Color, Is.EqualTo(controlDefaultBorderBrush?.Color));
        }

        public static void MouseOverBackground(FrameworkElement element)
        {
            var controlDefaultMouseOverBrush = element.TryFindResource(ControlDefaultMouseOverBrushResourceName) as SolidColorBrush;

            var rect = element.Element.Current.BoundingRectangle;
            MouseInput.MoveTo(rect.GetClickablePoint());

            var selectionButtonMap = element.Map<ButtonThemeMap>();
            var chrome = selectionButtonMap.Chrome;

            Assert.That(chrome.Background.Color, Is.EqualTo(controlDefaultMouseOverBrush?.Color));
        }

        public static void Background(FrameworkElement element)
        {
            var controlDefaultBackground = element.TryFindResource(ControlDefaultBackgroundBrushResourceName) as SolidColorBrush;

            Assert.That(element.Background.Color, Is.EqualTo(controlDefaultBackground?.Color));
        }
    }

    public class ButtonThemeAssert : FrameworkElementAssert
    {

    }
}
