namespace Orc.Automation.Tests;

using NUnit.Framework;
using ScenarioManagement;

public class LogScenarioAttribute<TLogger> : PropertyAttribute
    where TLogger : IAutomationScenarioLogger, new()
{
    public LogScenarioAttribute()
        : base(new TLogger())
    {
    }
}