namespace Orc.Automation
{
    using System.Windows;

    public abstract class NamedAutomationMethodRun : IAutomationMethodRun
    {
        public virtual string Name => GetType().Name;

        public virtual bool IsMatch(FrameworkElement owner, AutomationMethod method)
        {
            return Equals(method.Name, Name);
        }

        public abstract bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result);
    }
}
