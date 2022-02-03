namespace Orc.Automation.Tests
{
    using System.Windows.Media;
    using Controls;

    public interface IThemingControlState<in TElement> : IControlState<TElement>
        where TElement : FrameworkElement
    {
        Color? GetColor(TElement element, ColorType colorType);
    }
}
