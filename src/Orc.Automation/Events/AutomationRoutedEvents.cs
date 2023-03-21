namespace Orc.Automation;

using System.Windows;

public static class AutomationRoutedEvents
{
    public static readonly RoutedEvent AutomationMessageSentEvent
        = EventManager.RegisterRoutedEvent("AutomationMessageSent", RoutingStrategy.Bubble, typeof(AutomationMessageSentEventHandler), typeof(AutomationRoutedEvents));

    public static void AddAutomationMessageSentHandler(UIElement element, AutomationMessageSentEventHandler handler)
    {
        element.AddHandler(AutomationMessageSentEvent, handler);
    }

    public static void RemoveAutomationMessageSentHandler(UIElement element, AutomationMessageSentEventHandler handler)
    {
        element.RemoveHandler(AutomationMessageSentEvent, handler);
    }
}