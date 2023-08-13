namespace Orc.Automation.Controls;

using System.Windows.Automation.Peers;
using System.Windows.Controls;

public class AutomationInformer : ContentControl
{
    public AutomationInformer()
    {
        SetCurrentValue(FocusableProperty, false);
    }

    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new AutomationInformerPeer(this);
    }
}
