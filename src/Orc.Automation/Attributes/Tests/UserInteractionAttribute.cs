namespace Orc.Automation.Tests;

using System;
using System.Linq;
using System.Windows.Automation;
using MethodBoundaryAspect.Fody.Attributes;
using ScenarioManagement;

[AttributeUsage(AttributeTargets.Method)]
public class UserInteractionAttribute : OnMethodBoundaryAspect
{
    private readonly string _name;
    private readonly bool _isTryInteraction;

    private UserInteraction? _interaction;

    public UserInteractionAttribute(string name = "", bool isTryInteraction = false)
    {
        _name = name;
        _isTryInteraction = isTryInteraction;
    }

    public override void OnEntry(MethodExecutionArgs arg)
    {
        Wait.UntilResponsive();

        AutomationElement? element = null;
        if (arg.Instance is not AutomationControl control)
        {
            control = arg.Arguments.FirstOrDefault() as AutomationControl;
            element = control?.Element;
            if (control is null)
            {
                element = arg.Arguments.FirstOrDefault() as AutomationElement;
            }
        }

        if (element is null)
        {
            return;
        }

        var methodName = arg.Method.Name;
        if (_isTryInteraction)
        {
            methodName = methodName.Replace("Try", string.Empty);
        }

        var format = string.IsNullOrWhiteSpace(_name)
            ? FunctionNameFormatHelper.FormatMethodName(methodName, arg.Arguments)
            : _name;
        var name = string.Format(format, arg.Arguments);
        _interaction = new UserInteraction(name, element, control);
        ScenarioManager.InvokeBeforeInteraction(_interaction);
    }

    public override void OnExit(MethodExecutionArgs arg)
    {
        if (_interaction is null)
        {
            return;
        }

        if (!_isTryInteraction || (_isTryInteraction && Equals(arg.ReturnValue, true)))
        {
            ScenarioManager.InvokeAfterInteraction(_interaction);
            Wait.UntilResponsive();
        }
    }
}
