namespace Orc.Automation
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows;

    public interface IAutomationMethodRun
    {
        public abstract bool IsMatch(FrameworkElement owner, AutomationMethod method);
        public abstract bool TryInvoke(FrameworkElement owner, AutomationMethod method, [NotNullWhen(true)]out AutomationValue? result);
    }
}
