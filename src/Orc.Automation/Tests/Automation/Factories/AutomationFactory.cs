namespace Orc.Automation;

using System;
using Catel.IoC;
using Microsoft.Extensions.DependencyInjection;

public class AutomationFactory
{
    public T? Create<T>(object element)
        where T : AutomationBase
    {
        var serviceProvider = IoCContainer.ServiceProvider;

        if (element is AutomationControl control)
        {
            // TODO: This will throw exception, maybe need to introduce a TryCreateInstance method?
            return ActivatorUtilities.CreateInstance<T>(serviceProvider, control) ??
                ActivatorUtilities.CreateInstance<T>(serviceProvider, control.Element);
        }

        return ActivatorUtilities.CreateInstance<T>(serviceProvider, element);
    }

    public T CreateRequired<T>(object element)
        where T : AutomationBase
    {
        var instance = Create<T>(element);
        if (instance is null)
        {
            throw new InvalidOperationException($"Cannot create required instance of '{typeof(T).Name}'");
        }

        return instance;
    }
}
