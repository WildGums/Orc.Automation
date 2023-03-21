namespace Orc.Automation;

using System.Windows;

public class WindowModel : ContentControlModel
{
    public WindowModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }

    public string Title { get; set; }
    public bool AllowsTransparency { get; set; }
    public bool? DialogResult { get; set; }
    public double Left { get; set; }
    public WindowStyle WindowStyle { get; set; }
    public WindowState WindowState { get; set; }
    public WindowStartupLocation WindowStartupLocation { get; set; }
    public bool Topmost { get; set; }
    public double Top { get; set; }
    public SizeToContent SizeToContent { get; set; }
    public bool ShowInTaskbar { get; set; }
    public bool ShowActivated { get; set; }
    public ResizeMode ResizeMode { get; set; }
    public Rect RestoreBounds => _accessor.GetValue<Rect>();
    public bool IsActive => _accessor.GetValue<bool>();
}