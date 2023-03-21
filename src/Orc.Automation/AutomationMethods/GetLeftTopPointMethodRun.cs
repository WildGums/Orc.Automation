namespace Orc.Automation;

using System.Windows;

public class GetLeftTopMethodRun : NamedAutomationMethodRun
{
    public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
    {
        var point = owner.PointToScreen(new Point(0, 0));

        result = AutomationValue.FromValue(point);

        return true;
    }
}