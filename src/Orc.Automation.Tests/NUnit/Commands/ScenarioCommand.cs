namespace Orc.Automation.Tests;

using System;
using Catel;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Commands;
using ScenarioManagement;

public class ScenarioCommand : DelegatingTestCommand
{
    private readonly AutomationScenario _scenario;

    public ScenarioCommand(TestCommand innerCommand, AutomationScenario scenario)
        : base(innerCommand)
    {
        ArgumentNullException.ThrowIfNull(scenario);

        _scenario = scenario;
    }

    public override TestResult Execute(TestExecutionContext context)
    {
        using (new DisposableToken<ScenarioCommand>(this,
                   x => ScenarioManager.StartScenario(_scenario),
                   x => ScenarioManager.FinishCurrentScenario()))
        {
            context.CurrentResult = innerCommand.Execute(context);
            return context.CurrentResult;
        }
    }
}