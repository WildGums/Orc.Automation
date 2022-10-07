namespace Orc.Automation;

using System;
using System.Windows.Automation;

public class ControlAttribute : AutomationAttribute
{
    private string _className;

    public virtual string ClassName
    {
        get => _className ?? Class?.FullName;
        set => _className = value;
    }

    public ControlType? ControlType => AutomationHelper.GetControlType(ControlTypeName);
    public Type? Class { get; set; }
    public string? ControlTypeName { get; set; }
}
