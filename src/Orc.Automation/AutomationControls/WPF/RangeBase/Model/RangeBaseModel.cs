namespace Orc.Automation;

[ActiveAutomationModel]
public class RangeBaseModel : ControlModel
{
    public RangeBaseModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }

    public double Value { get; set; }
    public double Minimum { get; set; }
    public double Maximum { get; set; }
    public double LargeChange { get; set; }
    public double SmallChange { get; set; }
}