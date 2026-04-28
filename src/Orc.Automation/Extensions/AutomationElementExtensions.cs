namespace Orc.Automation;

using System;
using System.Windows.Automation;
using Catel.IoC;
using Microsoft.Extensions.DependencyInjection;

public static partial class AutomationElementExtensions
{
    public static TAutomationControl As<TAutomationControl>(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        var serviceProvider = IoCContainer.ServiceProvider;

        return ActivatorUtilities.CreateInstance<TAutomationControl>(serviceProvider, element);
    }

    public static bool IsVisible(this AutomationElement element)
    {
        return !IsOffscreen(element);
    }

    public static bool IsOffscreen(this AutomationElement element)
    {
        return (bool)element.GetCurrentPropertyValue(AutomationElement.IsOffscreenProperty);
    }
}
