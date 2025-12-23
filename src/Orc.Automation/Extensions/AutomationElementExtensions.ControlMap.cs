namespace Orc.Automation;

using System;
using System.Windows.Automation;
using Catel.IoC;
using Microsoft.Extensions.DependencyInjection;

public static partial class AutomationElementExtensions
{
    public static TTemplate CreateControlMap<TTemplate>(this AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return (TTemplate)element.CreateControlMap(typeof(TTemplate));
    }

    public static object CreateControlMap(this AutomationElement element, Type controlMapType)
    {
        ArgumentNullException.ThrowIfNull(element);

        var serviceProvider = IoCContainer.ServiceProvider;

        var template = ActivatorUtilities.CreateInstance(serviceProvider, controlMapType);

        InitializeControlMap(element, template);

        return template;
    }

    public static void InitializeControlMap(this AutomationElement element, object controlMap)
    {
        ArgumentNullException.ThrowIfNull(element);
        ArgumentNullException.ThrowIfNull(controlMap);

        TargetAttribute.ResolveTargetProperty(element, controlMap);
        TargetControlMapAttribute.Initialize(element, controlMap);
    }
}
