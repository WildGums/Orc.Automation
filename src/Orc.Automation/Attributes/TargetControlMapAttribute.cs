﻿namespace Orc.Automation
{
    using System;
    using System.Linq;
    using System.Windows.Automation;
    using Catel.IoC;

    [AttributeUsage(AttributeTargets.Property)]
    public class TargetControlMapAttribute : AutomationAttribute
    {
        public static void Initialize(AutomationElement element, object host)
        {
            var hostType = host.GetType();
            var targetElementMapProperty = hostType.GetProperties().FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(TargetControlMapAttribute)));
            if (targetElementMapProperty is null)
            {
                return;
            }

#pragma warning disable IDISP004 // Don't ignore created IDisposable
            var elementMap = element.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion(targetElementMapProperty.PropertyType);
#pragma warning restore IDISP004 // Don't ignore created IDisposable
            if (elementMap is null)
            {
                return;
            }

            targetElementMapProperty.SetValue(host, elementMap);

            element.InitializeControlMap(elementMap);
        }
    }
}
