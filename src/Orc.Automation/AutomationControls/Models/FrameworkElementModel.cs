namespace Orc.Automation;

using System.Windows;
using System.Windows.Media;

[ActiveAutomationModel]
public class FrameworkElementModel : UIElementModel
{
    public FrameworkElementModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public double ActualHeight => _accessor.GetValue<double>();
    public double ActualWidth => _accessor.GetValue<double>();

    //public ContextMenu ContextMenu {get; set;}
    public object DataContext { get; set; }
    public FlowDirection FlowDirection { get; set; }
    public bool ForceCursor { get; set; }
    public double Height { get; set; }
    public HorizontalAlignment HorizontalAlignment { get; set; }
    public bool IsInitialized => _accessor.GetValue<bool>();
    public bool IsLoaded => _accessor.GetValue<bool>();
    public Thickness Margin { get; set; }
    public double MaxHeight { get; set; }
    public double MaxWidth { get; set; }
    public double MinHeight { get; set; }
    public double MinWidth { get; set; }
    public object Tag { get; set; }
    public object ToolTip { get; set; }
    public bool UseLayoutRounding { get; set; }
    public VerticalAlignment VerticalAlignment { get; set; }
    public Visibility Visibility { get; set; }
    public double Width { get; set; }

    public SolidColorBrush Background { get; set; }

    public SolidColorBrush BorderBrush { get; set; }
}
