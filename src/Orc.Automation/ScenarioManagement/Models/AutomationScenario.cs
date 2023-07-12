namespace Orc.Automation.ScenarioManagement;

using System;
using System.Collections.Generic;
using System.Linq;
using Catel;

public class AutomationScenario
{
    private readonly List<AutomationScenarioStep> _startedSteps;
    private readonly List<AutomationScenarioStep> _finishedSteps;
    private readonly Stack<AutomationScenarioStep> _runningSteps;
    private readonly List<IAutomationScenarioLogger> _loggers = new();

    public AutomationScenario(string name, string description = "")
    {
        Argument.IsNotNullOrEmpty(() => name);

        Name = name;
        Description = description;

        _startedSteps = new List<AutomationScenarioStep>();
        _finishedSteps = new List<AutomationScenarioStep>();
        _runningSteps = new Stack<AutomationScenarioStep>();
    }

    public string Name { get; }
    public string Description { get; }
    public string? Suite { get; set; }

    public IReadOnlyList<AutomationScenarioStep> StartedSteps => _startedSteps;
    public IReadOnlyList<AutomationScenarioStep> FinishedSteps => _finishedSteps;

    public AutomationScenarioStep? CurrentStep => _runningSteps.Any() ? _runningSteps.Peek() : null;

    public IReadOnlyList<IAutomationScenarioLogger> Loggers => _loggers;

    public void AddLogger(IAutomationScenarioLogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);

        var loggerType = logger.GetType();
        if (_loggers.Any(x => x.GetType() == loggerType))
        {
            return;
        }

        _loggers.Add(logger);
    }

    public void InvokeBeforeInteraction(UserInteraction userInteraction)
    {
        ArgumentNullException.ThrowIfNull(userInteraction);

        CurrentStep?.AddInteraction(userInteraction);

        foreach (var logger in _loggers)
        {
            logger.LogBeforeUserInteraction(userInteraction);
        }
    }
    
    public void InvokeAfterInteraction(UserInteraction userInteraction)
    {
        ArgumentNullException.ThrowIfNull(userInteraction);

        foreach (var logger in _loggers)
        {
            logger.LogAfterUserInteraction(userInteraction);
        }
    }
    
    public void StartStep(AutomationScenarioStep step)
    {
        ArgumentNullException.ThrowIfNull(step);

        _startedSteps.Add(step);

        _runningSteps.Push(step);
        
        foreach (var logger in _loggers)
        {
            logger.LogStepStart(step);
        }
    }

    public void FinishCurrentStep()
    {
        if (CurrentStep is null)
        {
            return;
        }

        var finishingStep = _runningSteps.Pop();

        foreach (var logger in _loggers)
        {
            logger.LogStepFinish(finishingStep);
        }

        _finishedSteps.Add(finishingStep);
    }

    public void Start()
    {
        foreach (var logger in _loggers)
        {
            logger.LogScenarioStart(this);
        }
    }

    public void Finish()
    {
        foreach (var logger in _loggers)
        {
            logger.LogScenarioFinish(this);
        }
    }
}
