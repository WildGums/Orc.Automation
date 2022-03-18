namespace Orc.Automation
{
    using System.Windows;

    public delegate void AutomationMessageSentEventHandler(object sender, AutomationMessageSentEventArgs args);

    public class AutomationMessageSentEventArgs : RoutedEventArgs
    {
        public AutomationMessageSentEventArgs(RoutedEvent routedEvent)
            : base(routedEvent)
        {
        }

        public string Message { get; set; }
    }
}
