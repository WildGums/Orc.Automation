namespace Orc.Automation;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

public class AttachBehaviorMethodRun : IAutomationMethodRun
{
    public const string AttachBehaviorMethodPrefix = "AttachBehavior";

    public bool IsMatch(FrameworkElement owner, AutomationMethod method)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(method);

        var commandName = method.Name;
        return commandName.StartsWith(AttachBehaviorMethodPrefix);
    }

    public bool TryInvoke(FrameworkElement owner, AutomationMethod method, [NotNullWhen(true)]out AutomationValue? result)
    {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(method);

        var value = method.Parameters[0].ExtractValue() as Type;

        //TODO:Vladimir: just create non generic method in orc.theming
        var methodInfo = typeof(FrameworkElementExtensions).GetMethod("AttachBehavior");

        var genericMethod = methodInfo.MakeGenericMethod(value);
        genericMethod.Invoke(null, new[] { owner }); // No target, no arguments

        result = null;

        return true;
    }
}