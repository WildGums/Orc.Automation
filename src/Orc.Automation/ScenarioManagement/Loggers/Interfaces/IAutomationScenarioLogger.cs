namespace Orc.Automation.ScenarioManagement;

public interface IAutomationScenarioLogger
{
    void LogScenarioStart(AutomationScenario scenario);
    void LogScenarioFinish(AutomationScenario scenario);
    void LogStepStart(AutomationScenarioStep step);
    void LogStepFinish(AutomationScenarioStep step);
    void LogBeforeUserInteraction(UserInteraction userInteraction);
    void LogAfterUserInteraction(UserInteraction userInteraction);
}
