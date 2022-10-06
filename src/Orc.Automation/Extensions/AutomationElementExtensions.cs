namespace Orc.Automation
{
    using System;
    using System.Windows.Automation;
    using Catel;
    using Catel.IoC;

    public static partial class AutomationElementExtensions
    {
        public static TAutomationControl As<TAutomationControl>(this AutomationElement element)
        {
            ArgumentNullException.ThrowIfNull(element);

#pragma warning disable IDISP004 // Don't ignore created IDisposable
            return element.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion<TAutomationControl>(element);
#pragma warning restore IDISP004 // Don't ignore created IDisposable
        }

        public static bool IsVisible(this AutomationElement element)
        {
            if (element is null)
            {
                return false;
            }

            return !IsOffscreen(element);
        }

        public static bool IsOffscreen(this AutomationElement element)
        {
            if (element is null)
            {
                return true;
            }

            return (bool)element.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty);
        }
    }
}
