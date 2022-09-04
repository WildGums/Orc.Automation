namespace Orc.Automation.Services
{
    using System.Diagnostics;
    using System.Windows.Automation;

    public interface ISetupAutomationService
    {
        public AutomationSetup CurrentSetup { get; }

        AutomationSetup Setup(string executableFileLocation, Condition findMainWindowCondition, string args = null);
    }
}
