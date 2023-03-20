namespace Orc.Automation;

using System.Windows;
using System.Windows.Automation.Peers;
using Window = System.Windows.Window;

public class AutomationWindowPeerBase<TWindow> : AutomationControlPeerBase<TWindow>
    where TWindow : Window
{
    private readonly WindowAutomationPeer _windowPeer;

    public AutomationWindowPeerBase(TWindow owner) 
        : base(owner)
    {
        _windowPeer = new WindowAutomationPeer(owner);
    }

    public override object GetPattern(PatternInterface patternInterface)
    {
        if (patternInterface is PatternInterface.Window)
        {
            return _windowPeer;
        }

        return base.GetPattern(patternInterface);
    }

    protected override string GetNameCore() => _windowPeer.GetName();

    protected override AutomationControlType GetAutomationControlTypeCore() 
        => AutomationControlType.Window;

    protected override Rect GetBoundingRectangleCore()
    {
        return _windowPeer.GetBoundingRectangle();
    }
}