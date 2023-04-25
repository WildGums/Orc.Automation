namespace Orc.Automation.Tests;

using System;
using System.Linq;
using Gum.Controls.Automation.Tests;
using MethodBoundaryAspect.Fody.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class UserInteractionAttribute : OnMethodBoundaryAspect
{
    private readonly string _name;

    public UserInteractionAttribute(string name)
    {
        _name = name;
    }

    public override void OnEntry(MethodExecutionArgs arg)
    {
        if (arg.Instance is not AutomationControl element)
        {
            element = arg.Arguments.FirstOrDefault() as AutomationControl;
            if (element is null)
            {
                return;
            }
        }

        var interaction = new UserInteraction(_name, element.Element, element);
        ScenarioManager.InvokeInteraction(interaction);
    }

    public override void OnExit(MethodExecutionArgs arg)
    {
        ScenarioManager.FinishCurrentStep();
    }
}
