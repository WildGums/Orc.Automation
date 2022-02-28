namespace Orc.Automation;

using System.Windows;
using System.Windows.Media;

[AutomationAccessType]
public class FrameworkElementModel : AutomationControlModel
{
    public FrameworkElementModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public object DataContext { get; set; }

    public double Height { get; set; }

    public double Width { get; set; }

    public SolidColorBrush Background { get; set; }

    public SolidColorBrush BorderBrush { get; set; }

    public HorizontalAlignment HorizontalAlignment { get; set; }

    public VerticalAlignment VerticalAlignment { get; set; }

    public double ActualWidth => _accessor.GetValue<double>();

    public double ActualHeight => _accessor.GetValue<double>();

    public bool Focusable { get; set; }
}
