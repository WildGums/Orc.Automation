namespace Orc.Automation;

using System;
using System.Linq;
using System.Windows.Automation;
using Catel.IoC;
using Microsoft.Extensions.DependencyInjection;

[AttributeUsage(AttributeTargets.Property)]
public class TargetControlMapAttribute : AutomationAttribute
{
    public static void Initialize(AutomationElement element, object host)
    {
        ArgumentNullException.ThrowIfNull(element);
        ArgumentNullException.ThrowIfNull(host);

        var hostType = host.GetType();
        var targetElementMapProperty = hostType.GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(TargetControlMapAttribute)));
        if (targetElementMapProperty is null)
        {
            return;
        }

        var serviceProvider = IoCContainer.ServiceProvider;

        var elementMap = ActivatorUtilities.CreateInstance(serviceProvider, targetElementMapProperty.PropertyType);
        if (elementMap is null)
        {
            return;
        }

        targetElementMapProperty.SetValue(host, elementMap);

        element.InitializeControlMap(elementMap);
    }
}
