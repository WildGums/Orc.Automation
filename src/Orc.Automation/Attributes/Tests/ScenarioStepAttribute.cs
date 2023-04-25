namespace Gum.Controls.Automation.Tests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Automation;
using Catel;
using MethodBoundaryAspect.Fody.Attributes;
using Orc.Automation;

[AttributeUsage(AttributeTargets.Method)]
public class ScenarioStepAttribute : OnMethodBoundaryAspect
{
    private readonly string _name;
    private readonly string _description;

    public ScenarioStepAttribute(string name, string description = "")
    {
        _name = name;
        _description = description;
    }

    public override void OnEntry(MethodExecutionArgs arg)
    {
        //if (arg.Instance is not AutomationControl element)
        //{
        //    return;
        //}

        var step = new AutomationScenarioStep(_name, _description);
        ScenarioManager.StartStep(step);
    }

    public override void OnExit(MethodExecutionArgs arg)
    {
        //if (arg.Instance is not AutomationControl element)
        //{
        //    return;
        //}

        ScenarioManager.FinishCurrentStep();
    }

//    private void Screen(AutomationControl control)
//    {
//        if (ScenarioManager.IsRecording)
//        {
//            return;
//        }

//        try
//        {
//            Wait.UntilResponsive();

//            var bounds = control.BoundingRectangle;
//#pragma warning disable IDISP001 // Dispose created
//            var captureBitmap = new Bitmap((int)bounds.Width, (int)bounds.Height, PixelFormat.Format32bppArgb);
//#pragma warning restore IDISP001 // Dispose created
//            var captureGraphics = Graphics.FromImage(captureBitmap);
//            captureGraphics.CopyFromScreen((int)bounds.X, (int)bounds.Y, 0, 0, new Size((int)bounds.Width, (int)bounds.Height));

//            captureGraphics.Dispose();
//            captureBitmap.Save(@$"C:\Temp\{_description}-{_step++}.jpg", ImageFormat.Jpeg);
//        }
//        catch (Exception ex)
//        {
//            Console.Write(ex.ToString());
//        }
//    }
}

public static class ScenarioManager
{
    private static AutomationScenario? _currentScenario;

    public static AutomationScenario? CurrentScenario => _currentScenario;

    public static bool IsRecording => CurrentScenario is not null;

    public static void StartScenario(AutomationScenario scenario)
    {
        ArgumentNullException.ThrowIfNull(scenario);

        _currentScenario = scenario;
        _currentScenario.Start();
    }

    public static void FinishCurrentScenario()
    {
        _currentScenario?.Finish();
        _currentScenario = null;
    }

    public static void StartStep(AutomationScenarioStep step)
    {
        ArgumentNullException.ThrowIfNull(step);

        if (_currentScenario is null)
        {
            return;
        }

        _currentScenario.StartStep(step);
    }

    public static void FinishCurrentStep()
    {
        if (_currentScenario is null)
        {
            return;
        }

        _currentScenario.FinishCurrentStep();
    }

    public static void InvokeInteraction(UserInteraction interaction)
    {
        ArgumentNullException.ThrowIfNull(interaction);

        _currentScenario?.InvokeInteraction(interaction);
    }
}

public class AutomationScenario
{
    private readonly List<AutomationScenarioStep> _finishedSteps;
    private AutomationScenarioStep? _currentStep;
    
    public AutomationScenario(string name, string description = "")
    {
        Argument.IsNotNullOrEmpty(() => name);

        Name = name;
        Description = description;

        _finishedSteps = new List<AutomationScenarioStep>();
    }

    public string Name { get; }
    public string Description { get; }

    public IReadOnlyList<AutomationScenarioStep> FinishedSteps => _finishedSteps;

    public AutomationScenarioStep? CurrentStep => _currentStep;

    public IReadOnlyList<IAutomationScenarioLogger> Loggers { get; set; } = Array.Empty<IAutomationScenarioLogger>();

    public void InvokeInteraction(UserInteraction userInteraction)
    {
        ArgumentNullException.ThrowIfNull(userInteraction);

        _currentStep?.AddInteraction(userInteraction);

        foreach (var logger in Loggers)
        {
            logger.LogUserInteraction(userInteraction);
        }
    }

    public void StartStep(AutomationScenarioStep step)
    {
        ArgumentNullException.ThrowIfNull(step);

        _currentStep = step;
        
        foreach (var logger in Loggers)
        {
            logger.LogStepStart(_currentStep);
        }
    }

    public void FinishCurrentStep()
    {
        if (_currentStep is null)
        {
            return;
        }

        foreach (var logger in Loggers)
        {
            logger.LogStepFinish(_currentStep);
        }

        _finishedSteps.Add(_currentStep);
        _currentStep = null;
    }

    public void Start()
    {
        foreach (var logger in Loggers)
        {
            logger.LogScenarioStart(this);
        }
    }

    public void Finish()
    {
        foreach (var logger in Loggers)
        {
            logger.LogScenarioFinish(this);
        }
    }
}

public class AutomationScenarioStep
{
    private readonly List<UserInteraction> _interactions;

    public AutomationScenarioStep(string name, string description = "")
    {
        Argument.IsNotNullOrEmpty(() => name);

        Name = name;
        Description = description;

        _interactions = new List<UserInteraction>();
    }

    public string Name { get; }
    public string Description { get; }

    public IReadOnlyList<UserInteraction> Interactions => _interactions;

    public void AddInteraction(UserInteraction userInteraction)
    {
        ArgumentNullException.ThrowIfNull(userInteraction);

        _interactions.Add(userInteraction);
    }
}

public class UserInteraction
{
    public UserInteraction(string name, AutomationElement automationElement, AutomationControl? control = null)
    {
        Argument.IsNotNullOrEmpty(() => name);

        Name = name;

        var currentInformation = automationElement.Current;

        InteractionArea = currentInformation.BoundingRectangle;
        ControlType = currentInformation.ControlType;
        ControlName = currentInformation.Name;
        ControlClassName = currentInformation.ClassName;
        AutomationId = currentInformation.AutomationId;
        AutomationElement = automationElement;
        AutomationControl = control;
    }

    public string Name { get; }
    public string ControlName { get; }
    public string ControlClassName { get; }
    public string AutomationId { get; }
    public Rect InteractionArea { get; }
    public ControlType ControlType { get; }
    public AutomationElement AutomationElement { get; }
    public AutomationControl? AutomationControl { get; }
}

public interface IAutomationScenarioLogger
{
    void LogScenarioStart(AutomationScenario scenario);
    void LogScenarioFinish(AutomationScenario scenario);
    void LogStepStart(AutomationScenarioStep step);
    void LogStepFinish(AutomationScenarioStep step);
    void LogUserInteraction(UserInteraction userInteraction);
}

public class TextAutomationScenarioLogger : IAutomationScenarioLogger
{
    private const string LogFilePath = "D:\\Logs\\Log.txt";

    public void LogScenarioStart(AutomationScenario scenario)
    {
        File.AppendAllText(LogFilePath, $"START: {scenario.Name}\r\n");
    }

    public void LogScenarioFinish(AutomationScenario scenario)
    {
        File.AppendAllText(LogFilePath, $"FINISH: {scenario.Name}\r\n");
    }

    public void LogStepStart(AutomationScenarioStep step)
    {
        File.AppendAllText(LogFilePath, $"Start step: {step.Name}\r\n");
    }

    public void LogStepFinish(AutomationScenarioStep step)
    {
        File.AppendAllText(LogFilePath, $"Finish step: {step.Name}\r\n");
    }

    public void LogUserInteraction(UserInteraction userInteraction)
    {
        File.AppendAllText(LogFilePath, $"    Interaction: {userInteraction.Name}\r\n");
    }
}
