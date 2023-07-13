namespace Orc.Automation.ScenarioManagement;

using System;

public static class ScenarioManager
{
    private static AutomationScenario? _currentScenario;

    public static AutomationScenario? CurrentScenario => _currentScenario;

    public static bool IsRecordingStepsSuspended { get; set; }

    public static void StartScenario(AutomationScenario scenario)
    {
        ArgumentNullException.ThrowIfNull(scenario);

        IsRecordingStepsSuspended = false;

        _currentScenario = scenario;
        _currentScenario.Start();
    }

    public static void FinishCurrentScenario()
    {
        _currentScenario?.Finish();
        _currentScenario = null;

        IsRecordingStepsSuspended = true;
    }

    public static void StartStep(AutomationScenarioStep step)
    {
        ArgumentNullException.ThrowIfNull(step);

        if (IsRecordingStepsSuspended)
        {
            return;
        }

        if (_currentScenario is null)
        {
            return;
        }

        _currentScenario.StartStep(step);
    }

    public static void FinishCurrentStep()
    {
        if (IsRecordingStepsSuspended)
        {
            return;
        }

        if (_currentScenario is null)
        {
            return;
        }

        _currentScenario.FinishCurrentStep();
    }

    public static void InvokeBeforeInteraction(UserInteraction interaction)
    {
        ArgumentNullException.ThrowIfNull(interaction);

        if (IsRecordingStepsSuspended)
        {
            return;
        }

        _currentScenario?.InvokeBeforeInteraction(interaction);
    }

    public static void InvokeAfterInteraction(UserInteraction interaction)
    {
        ArgumentNullException.ThrowIfNull(interaction);

        if (IsRecordingStepsSuspended)
        {
            return;
        }

        _currentScenario?.InvokeAfterInteraction(interaction);
    }
}
