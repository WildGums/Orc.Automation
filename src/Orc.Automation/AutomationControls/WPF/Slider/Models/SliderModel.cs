namespace Orc.Automation;

using System.Windows.Controls;
using System.Windows.Controls.Primitives;

[ActiveAutomationModel]
public class SliderModel : RangeBaseModel
{
    public SliderModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }

    public Orientation Orientation { get; set; }
    public double SelectionStart { get; set; }
    public AutoToolTipPlacement AutoToolTipPlacement { get; set; }
    public int AutoToolTipPrecision { get; set; }
    public int Delay { get; set; }
    public TickPlacement TickPlacement { get; set; }
    public double TickFrequency { get; set; }
    public double SelectionEnd { get; set; }
    public bool IsDirectionReversed { get; set; }
    public bool IsSnapToTickEnabled { get; set; }
    public bool IsMoveToPointEnabled { get; set; }
    public bool IsSelectionRangeEnabled { get; set; }
    public int Interval { get; set; }
}