namespace Orc.Automation.Tests;

using System;
using System.Linq;
using System.Windows.Automation;
using Catel;
using MethodBoundaryAspect.Fody.Attributes;
using Orc.Automation;
using ScenarioManagement;

[AttributeUsage(AttributeTargets.Method)]
public class ScenarioStepAttribute : OnMethodBoundaryAspect
{
    private readonly string _name;
    private readonly string _description;

    public ScenarioStepAttribute(string name = "", string description = "")
    {
        _name = name;
        _description = description;
    }

    public int EntryTimeOut { get; set; } = 200;
    public int ExitTimeOut { get; set; } = 200;

    public override void OnEntry(MethodExecutionArgs arg)
    {
        Wait.UntilInputProcessed(EntryTimeOut);

        AutomationElement? element = null;
        if (arg.Instance is not AutomationControl control)
        {
            control = arg.Arguments.FirstOrDefault() as AutomationControl;
            element = control?.Element;
            if (element is null)
            {
                element = arg.Arguments.FirstOrDefault() as AutomationElement;
            }
        }
        else
        {
            element = control.Element;
        }

        var stepNameFormat = string.IsNullOrWhiteSpace(_name) 
            ? FunctionNameFormatHelper.FormatMethodName(arg.Method.Name, arg.Arguments)
            : _name;
        var stepName = string.Format(stepNameFormat, arg.Arguments);

        var step = new AutomationScenarioStep(stepName, _description, element);
        ScenarioManager.StartStep(step);
    }

    public override void OnExit(MethodExecutionArgs arg)
    {
        Wait.UntilInputProcessed(ExitTimeOut);

        ScenarioManager.FinishCurrentStep();

        Wait.UntilResponsive();
    }
}
