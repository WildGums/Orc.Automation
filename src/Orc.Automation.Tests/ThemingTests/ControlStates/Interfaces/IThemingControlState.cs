namespace Orc.Automation.Tests;

using System.Windows.Media;

public interface IThemingControlState<in TElement> : IControlState<TElement>
    where TElement : AutomationControl
{
    Color? GetColor(TElement element, ColorType colorType);
}