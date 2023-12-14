namespace Orc.Automation;

using System.Collections;

[ActiveAutomationModel]
public class ItemsControlModel : ControlModel
{
    public ItemsControlModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    [ActiveAutomationProperty]
    public int AlternationCount { get; set; }
    [ActiveAutomationProperty]
    public bool IsTextSearchEnabled { get; set; }
    [ActiveAutomationProperty]
    public bool IsTextSearchCaseSensitive { get; set; }
    [ActiveAutomationProperty]
    public bool IsGrouping => _accessor.GetValue<bool>();
    [ActiveAutomationProperty]
    public bool HasItems => _accessor.GetValue<bool>();
    [ActiveAutomationProperty]
    public string DisplayMemberPath { get; set; }
    [ActiveAutomationProperty]
    public string ItemStringFormat { get; set; }

    //TODO:
    public IEnumerable ItemsSource
    {
        get => _accessor.ExecuteAutomationMethod<GetItemSourceAutomationMethodRun>() as IEnumerable;
    }
}