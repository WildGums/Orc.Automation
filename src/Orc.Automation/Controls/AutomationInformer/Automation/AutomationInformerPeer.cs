namespace Orc.Automation
{
    using System.Windows;
    using Recording;

    public class AutomationInformerPeer : ControlRunMethodAutomationPeerBase<Controls.AutomationInformer>
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

            MessageBox.Show(items.Count.ToString());

            items.Clear();
        }
    }
}
