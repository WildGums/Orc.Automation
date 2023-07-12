namespace Orc.Automation.ScenarioManagement;

using System;
using System.Windows;
using System.Windows.Automation;
using Catel;
using Orc.Automation;

public class UserInteraction
{
    public UserInteraction(string name, AutomationElement automationElement, AutomationControl? control = null)
    {
        Argument.IsNotNullOrEmpty(() => name);
        ArgumentNullException.ThrowIfNull(automationElement);

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
