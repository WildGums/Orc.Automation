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

    public double Height { get; set; }

    public double Width { get; set; }

    public SolidColorBrush Background { get; set; }

    public SolidColorBrush BorderBrush { get; set; }

    public HorizontalAlignment HorizontalAlignment { get; set; }

    public VerticalAlignment VerticalAlignment { get; set; }

    public double ActualWidth { get; }

    public double ActualHeight { get; }

    public bool Focusable { get; set; }
}
