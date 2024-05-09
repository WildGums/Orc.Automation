#nullable enable
namespace Orc.Automation.Tests;

using System;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using ScenarioManagement;

[AttributeUsage(AttributeTargets.Method)]
public class ScenarioTestCaseAttribute : TestCaseAttribute, IWrapTestMethod
{
    private readonly string _scenarioName;

    public ScenarioTestCaseAttribute(string scenarioName, params object[] arguments)
        : base(arguments) =>
        _scenarioName = scenarioName;

    public string? ScenarioSuite { get; set; }

    public TestCommand Wrap(TestCommand command)
    {
        const string scenarioAlreadyRunKey = "ScenarioAlreadyRun";

        var testProperties = command.Test.Parent?.Properties;
        var description = string.Empty;
        var loggers = Array.Empty<IAutomationScenarioLogger>();

        if (testProperties is not null)
        {
            if (testProperties.ContainsKey(scenarioAlreadyRunKey))
            {
                return command;
            }

            if (testProperties.ContainsKey(PropertyNames.Description))
            {
                description = testProperties[PropertyNames.Description]
                    .OfType<string>()
                    .FirstOrDefault() ?? string.Empty;
            }

            var logScenarioKey = typeof(LogScenarioAttribute<>).Name;
            if (testProperties.ContainsKey(logScenarioKey))
            {
                loggers = testProperties[logScenarioKey]
                    .OfType<IAutomationScenarioLogger>()
                    .ToArray();
            }

            testProperties.Add(scenarioAlreadyRunKey, true);
        }

        var scenario = new AutomationScenario(_scenarioName, description)
        {
            Suite = ScenarioSuite
        };

        foreach (var logger in loggers)
        {
            scenario.AddLogger(logger);
        }

        return new ScenarioCommand(command, scenario);
    }
}