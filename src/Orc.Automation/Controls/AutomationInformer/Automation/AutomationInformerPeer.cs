namespace Orc.Automation;

using System.Reflection;
using System;
using System.IO;
using Recording;
using System.Collections.Generic;

public class AutomationInformerPeer : AutomationControlPeerBase<Controls.AutomationInformer>
{
    public AutomationInformerPeer(Controls.AutomationInformer owner)
        : base(owner)
    {
    }

    [AutomationMethod]
    public void StartRecord()
    {
        EventListener.StartListen();
    }

    [AutomationMethod]
    public void StopRecord()
    {
        EventListener.StopListen();

        var items = EventListener.Events;

        System.Windows.MessageBox.Show(items.Count.ToString() ?? string.Empty);

        items.Clear();
    }
}
