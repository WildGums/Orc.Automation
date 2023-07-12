#nullable enable
namespace Orc.Automation.ScenarioManagement.Tests;

using System.Diagnostics.CodeAnalysis;
using System.IO;
using NUnit.Framework;

public class TextAutomationScenarioLogger : IAutomationScenarioLogger
{
    private readonly string _logRootDirectory;

    public TextAutomationScenarioLogger()
    {
        _logRootDirectory = Path.Combine(TestContext.CurrentContext.TestDirectory, "ScenarioLog");
        Directory.CreateDirectory(_logRootDirectory);
    }

    public void LogScenarioStart(AutomationScenario scenario)
    {
        if (!TryGetCurrentLogFilePath(out var logFilePath))
        {
            return;
        }

        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
        }
            
        File.WriteAllText(logFilePath, $"START: {scenario.Name}\r\n");
    }

    public void LogScenarioFinish(AutomationScenario scenario)
    {
        if (TryGetCurrentLogFilePath(out var logFilePath))
        {
            File.AppendAllText(logFilePath, $"FINISH: {scenario.Name}\r\n");
        }
    }

    public void LogStepStart(AutomationScenarioStep step)
    {
        if (TryGetCurrentLogFilePath(out var logFilePath))
        {
            File.AppendAllText(logFilePath, $"Start step: {step.Name}\r\n");
        }
    }

    public void LogStepFinish(AutomationScenarioStep step)
    {
        if (TryGetCurrentLogFilePath(out var logFilePath))
        {
            File.AppendAllText(logFilePath, $"Finish step: {step.Name}\r\n");
        }
    }

    public void LogBeforeUserInteraction(UserInteraction userInteraction)
    {
        
    }

    public void LogAfterUserInteraction(UserInteraction userInteraction)
    {
        if (TryGetCurrentLogFilePath(out var logFilePath))
        {
            File.AppendAllText(logFilePath, $"    Interaction: {userInteraction.Name}\r\n");
        }
    }

    private bool TryGetCurrentLogFilePath([NotNullWhen(true)] out string? logFilePath)
    {
        logFilePath = null;

        var scenario = ScenarioManager.CurrentScenario;
        if (scenario is null)
        {
            return false;
        }

        logFilePath = Path.Combine(_logRootDirectory, $"{scenario.Name}.log");
        return true;
    }
}
