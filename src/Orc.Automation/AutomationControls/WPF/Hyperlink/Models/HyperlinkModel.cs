namespace Orc.Automation;

using System;
using System.Windows.Input;

[ActiveAutomationModel]
public class HyperlinkModel : FrameworkElementModel
{
    public HyperlinkModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public Uri NavigateUri { get; set; }
    public ICommand Command { get; set; }
    public object CommandParameter { get; set; }
    public string TargetName { get; set; }
}