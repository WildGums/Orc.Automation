namespace Orc.Automation;

using System.Diagnostics.CodeAnalysis;
using System.Windows;

public abstract class ExtensionAutomationMethodBase<TOwner> : NamedAutomationMethodRun
    where TOwner : FrameworkElement
{
    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, [NotNullWhen(true)]out AutomationValue? result)
    {
        result = AutomationValue.FromValue(false);
        return owner is TOwner ownerElement && TryInvoke(ownerElement, method, out result);
    }

    protected abstract bool TryInvoke(TOwner owner, AutomationMethod method, [NotNullWhen(true)] out AutomationValue? result);
}
