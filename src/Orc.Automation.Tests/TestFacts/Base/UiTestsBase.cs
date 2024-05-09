namespace Orc.Automation.Tests;

using System.Windows.Automation;
using Catel.IoC;
using NUnit.Framework;
using Services;

public abstract class UiTestsBase
{
    private ISetupAutomationService _setupAutomationService;
#pragma warning disable IDISP006 // Don't ignore created IDisposable.
    protected AutomationSetup Setup { get; private set; }
#pragma warning disable IDISP006 // Don't ignore created IDisposable.
    protected virtual string ExecutablePath => string.Empty;
    protected virtual string Args => null;
    protected virtual string MainWindowAutomationId => string.Empty;

    protected virtual Condition FindMainWindowCondition =>
        new PropertyCondition(AutomationElement.AutomationIdProperty, MainWindowAutomationId);

    protected ISetupAutomationService SetupAutomationService =>
        _setupAutomationService ??= CreateSetupAutomationService();

    public virtual void SetUp()
    {
#pragma warning disable IDISP003 // Don't ignore created IDisposable.
        Setup = SetupAutomationService?.Setup(ExecutablePath, FindMainWindowCondition, Args);
#pragma warning disable IDISP003 // Don't ignore created IDisposable.

        Assert.That(Setup, Is.Not.Null);
    }

    public virtual void TearDown()
    {
        Setup?.Dispose();
        Setup = null;
    }

    protected virtual ISetupAutomationService CreateSetupAutomationService()
    {
#pragma warning disable IDISP004 // Don't ignore created IDisposable.
        return this.GetServiceLocator().ResolveType<ISetupAutomationService>();
#pragma warning restore IDISP004 // Don't ignore created IDisposable.
    }
}
