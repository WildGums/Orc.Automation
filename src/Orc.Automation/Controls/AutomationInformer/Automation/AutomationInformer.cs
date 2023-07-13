namespace Orc.Automation;

using System.Windows.Automation;
using System.Windows.Markup;
using Controls;

[AutomatedControl(Class = typeof(Controls.AutomationInformer))]
public class AutomationInformer : FrameworkElement
{
    public AutomationInformer(AutomationElement element)
        : base(element)
    {
    }

    public void StartRecord()
    {
        Execute(nameof(AutomationInformerPeer.StartRecord));
    }

    public void StopRecord()
    {
        Execute(nameof(AutomationInformerPeer.StopRecord));
    }
}
