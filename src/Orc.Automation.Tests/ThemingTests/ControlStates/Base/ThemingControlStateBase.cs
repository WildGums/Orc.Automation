namespace Orc.Automation.Tests;

using System.Windows.Media;

public abstract class ThemingControlStateBase<TElement> : ControlStateBase<TElement>, IThemingControlState<TElement>
    where TElement : AutomationControl
{
    private string _stateName;

    public Color? GetColor(TElement element, ColorType colorType)
    {
        //TODO: Vladimir, not clear
        _stateName ??= GetType().Name.Replace("ThemingControlState`1", string.Empty);

        var resourceName = $"Orc.Brushes.Control.{_stateName}.{colorType}";

        return (element.TryFindResource(resourceName) as SolidColorBrush)?.Color;
    }
}