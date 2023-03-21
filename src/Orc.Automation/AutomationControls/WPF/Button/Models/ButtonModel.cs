namespace Orc.Automation;

[ActiveAutomationModel]
public class ButtonModel : ButtonBaseModel
{
    public ButtonModel(AutomationElementAccessor accessor)
        : base(accessor)
    {
    }
        
    public bool IsCancel { get; set; }
    public bool IsDefault { get; set; }
    public bool IsDefaulted => _accessor.GetValue<bool>();
}