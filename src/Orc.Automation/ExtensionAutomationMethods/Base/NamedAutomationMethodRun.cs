namespace Orc.Automation
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    public abstract class NamedAutomationMethodRun : IAutomationMethodRun
    {
        public virtual string Name => GetType().Name;

        public virtual bool IsMatch(FrameworkElement owner, AutomationMethod method)
        {
            ArgumentNullException.ThrowIfNull(owner);
            ArgumentNullException.ThrowIfNull(method);

            return Equals(method.Name, Name);
        }

        public abstract bool TryInvoke(FrameworkElement owner, AutomationMethod method, [NotNullWhen(true)] out AutomationValue result);
    }
}
