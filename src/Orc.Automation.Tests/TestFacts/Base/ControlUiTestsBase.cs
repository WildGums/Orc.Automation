namespace Orc.Automation.Tests;

using System.Threading;
using Controls;
using NUnit.Framework;
using FrameworkElement = System.Windows.FrameworkElement;

public abstract class ControlUiTestsBase<TControl> : UiTestFactsBase
    where TControl : FrameworkElement
{
    protected override string ExecutablePath =>
        @$"{TestContext.CurrentContext.TestDirectory}\..\..\..\..\Tools\TestHost\Orc.Automation.Host.exe";

    protected override string MainWindowAutomationId => "AutomationHost";

    [SetUp]
    public virtual void SetUpTest()
    {
        var window = Setup.MainWindow;

        var testHost = window.Find<TestHostAutomationControl>();
        if (testHost is null)
        {
            Assert.Fail("Can't find Test host");
        }

        BeforeLoadingControl(testHost);

        Assert.That(TryLoadControl(testHost, out var testedControlAutomationId));

        AfterLoadingControl(testHost);

        Thread.Sleep(200);

        InitializeTarget(testedControlAutomationId);
    }

    [TearDown]
    public virtual void TearDownTest()
    {
        var testHost = Setup.MainWindow.Find<TestHostAutomationControl>();

        testHost?.ClearControls();
    }

    protected virtual bool TryLoadControl(TestHostAutomationControl testHost, out string testedControlAutomationId)
    {
        var controlType = typeof(TControl);

        return testHost.TryLoadControl(controlType, out testedControlAutomationId,
            $"pack://application:,,,/{controlType.Assembly.GetName().Name};component/Themes/Generic.xaml");
    }

    protected virtual void BeforeLoadingControl(TestHostAutomationControl testHost)
    {
    }

    protected virtual void AfterLoadingControl(TestHostAutomationControl testHost)
    {
    }

    protected virtual void InitializeTarget(string id)
    {
        var window = Setup.MainWindow;

        var testHost = window.Find<TestHostAutomationControl>();
        if (testHost is null)
        {
            return;
        }

        var target = testHost.Find(id);
        if (target is null)
        {
            Assert.Fail("Can't find target control");
        }

        target.InitializeControlMap(this);
    }
}