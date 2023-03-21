namespace Orc.Automation;

using System;
using System.Windows.Automation;
using Catel.IoC;

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

#pragma warning disable IDISP004 // Don't ignore created IDisposable
        var template = element.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(controlMapType);
#pragma warning restore IDISP004 // Don't ignore created IDisposable

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