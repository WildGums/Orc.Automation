namespace Orc.Automation;

[ActiveAutomationModel]
public class RadioButtonModel : ButtonBaseModel
{
    public RadioButtonModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public bool IsToggled { get; set; }
}
