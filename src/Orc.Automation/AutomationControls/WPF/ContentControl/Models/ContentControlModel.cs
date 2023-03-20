namespace Orc.Automation;

[ActiveAutomationModel]
public class ContentControlModel : ControlModel
{
    public ContentControlModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }

    public object Content { get; set; }
    public string ContentStringFormat { get; set; }
    public bool HasContent { get; set; }
}