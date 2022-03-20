namespace Orc.Automation;

using System.Windows;
using System.Windows.Media;

[ActiveAutomationModel]
public class UIElementModel : AutomationControlModel
{
    public UIElementModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public bool IsEnabled { get; set; }
    public bool AllowDrop { get; set; }
    public bool AreAnyTouchesCaptured { get; set; }
    public bool AreAnyTouchesCapturedWithin { get; set; }
    public bool AreAnyTouchesDirectlyOver { get; set; }
    public bool AreAnyTouchesOver { get; set; }
    public Size DesiredSize { get; set; }
    public bool Focusable { get; set; }
    public bool HasAnimatedProperties => _accessor.GetValue<bool>();
    public bool IsArrangeValid => _accessor.GetValue<bool>();
    public bool IsFocused => _accessor.GetValue<bool>();
    public bool IsHitTestVisible { get; set; }
    public bool IsKeyboardFocused => _accessor.GetValue<bool>();
    public bool IsKeyboardFocusWithin => _accessor.GetValue<bool>();
    public bool IsManipulationEnabled { get; set; }
    public bool IsMeasureValid => _accessor.GetValue<bool>();
    public bool IsMouseCaptured => _accessor.GetValue<bool>();
    public bool IsMouseCaptureWithin => _accessor.GetValue<bool>();
    public bool IsMouseDirectlyOver => _accessor.GetValue<bool>();
    public bool IsMouseOver => _accessor.GetValue<bool>();
    public bool IsSealed => _accessor.GetValue<bool>();
    public bool IsStylusCaptured => _accessor.GetValue<bool>();
    public bool IsStylusCaptureWithin => _accessor.GetValue<bool>();
    public bool IsStylusDirectlyOver => _accessor.GetValue<bool>();
    public bool IsStylusOver => _accessor.GetValue<bool>();

    /// <summary>
    /// Use IsVisible()
    /// </summary>
    public bool IsVisible => _accessor.GetValue<bool>();
    public double Opacity { get; set; }
    public SolidColorBrush OpacityMask { get; set; }
    public Size RenderSize { get; set; }
    public bool SnapsToDevicePixels { get; set; }

}