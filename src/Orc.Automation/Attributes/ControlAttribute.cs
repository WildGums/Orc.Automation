namespace Orc.Automation;

using System;
using System.Windows.Automation;
using Catel.Reflection;

public class ControlAttribute : AutomationAttribute
{
    private string? _className;

    public virtual string ClassName
    {
        get => _className ?? Class?.GetSafeFullName() ?? string.Empty;
        set => _className = value;
    }

    public ControlType? ControlType => AutomationHelper.GetControlType(ControlTypeName);
    public Type? Class { get; set; }
    public string? ControlTypeName { get; set; }
}
