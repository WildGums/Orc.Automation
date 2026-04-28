namespace Orc.Automation.Services;

using System.Windows.Automation;
using Catel.IoC;

public interface ISetupAutomationService : IConstructAtStartup
{
    public AutomationSetup? CurrentSetup { get; }

    AutomationSetup Setup(string executableFileLocation, Condition findMainWindowCondition, string? args = null);
}
