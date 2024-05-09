namespace Orc.Automation.Tests;

using FrameworkElement = System.Windows.FrameworkElement;

[ObsoleteEx(RemoveInVersion = "6.0.0", TreatAsErrorFromVersion = "5.0.0",
    ReplacementTypeOrMember = nameof(ControlUiTestsBase<TControl>))]
public abstract class ControlUiTestFactsBase<TControl> : ControlUiTestsBase<TControl>
    where TControl : FrameworkElement
{

}
