namespace Orc.Automation.ScenarioManagement;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation;
using Catel;

public class AutomationScenarioStep
{
    private readonly List<UserInteraction> _interactions;

    public AutomationScenarioStep(string name, string description = "", AutomationElement? element = null)
    {
        Argument.IsNotNullOrEmpty(() => name);

        Name = name;
        Description = description;
        AutomationElement = element; 
        InteractionArea = Controls.Window.MainWindow?.Current.BoundingRectangle ?? Rect.Empty;

        _interactions = new List<UserInteraction>();
    }

    public string Name { get; }
    public string Description { get; }
    public int Index { get; set; }
    public AutomationElement? AutomationElement { get; }
    public Rect InteractionArea { get; }

    public IReadOnlyList<UserInteraction> Interactions => _interactions;

    public void AddInteraction(UserInteraction userInteraction)
    {
        ArgumentNullException.ThrowIfNull(userInteraction);

        _interactions.Add(userInteraction);
    }
}
