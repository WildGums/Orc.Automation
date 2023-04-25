namespace Orc.Automation.Tests;

using System;
using System.Linq;
using Gum.Controls.Automation.Tests;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Commands;
using NUnit.Framework;

public class ScenarioAttribute : NUnitAttribute, IWrapSetUpTearDown
{
    private readonly string _scenarioName;
    private readonly string _description;
    private readonly Type[] _loggerTypes;

    public ScenarioAttribute(string scenarioName, string description = "", params Type[] loggerTypes)
    {
        _scenarioName = scenarioName;
        _description = description;
        _loggerTypes = loggerTypes;
    }

    public TestCommand Wrap(TestCommand command)
    {
        var loggers = _loggerTypes?.Select(x => (IAutomationScenarioLogger)Activator.CreateInstance(x)).ToArray()
                      ?? Array.Empty<IAutomationScenarioLogger>();

        var scenario = new AutomationScenario(_scenarioName, _description)
        {
            Loggers = loggers
        };

        return new ScenarioCommand(command, scenario);
    }
}
