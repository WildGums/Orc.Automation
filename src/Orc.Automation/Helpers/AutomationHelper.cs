﻿namespace Orc.Automation;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Automation;
using Catel.IoC;
using Catel.Reflection;

public static class AutomationHelper
{
    private const int MaxStackTraceLookUp = 5;

    public static bool IsActiveModelControl(AutomationElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.Current.ClassName.Contains(NameConventions.ActiveModelControlClassNameSuffix);
    }

    public static string GetActiveControlClassName(Type controlType)
    {
        return $"{controlType.GetSafeFullName()}{NameConventions.ActiveModelControlClassNameSuffix}";
    }

    public static ControlType? GetControlType(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var controlType = GetControlType(type.Name);
        if (controlType is not null)
        {
            return controlType;
        }

        var automatedControlAttribute = type.GetCustomAttribute<AutomatedControlAttribute>() ?? type.GetCustomAttribute<ControlAttribute>();
        if (automatedControlAttribute is null)
        {
            return null;
        }

        controlType = automatedControlAttribute.ControlType;
        return controlType;
    }

    public static string? GetControlClassName(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        var automatedControlAttribute = type.GetCustomAttribute<AutomatedControlAttribute>() ?? type.GetCustomAttribute<ControlAttribute>();
        if (automatedControlAttribute is null)
        {
            return null;
        }

        var className = automatedControlAttribute.ClassName;
        return string.IsNullOrWhiteSpace(className) ? null : className;
    }

    public static ControlType? GetControlType(string? controlTypeName)
    {
        if (!string.IsNullOrWhiteSpace(controlTypeName))
        {
            return typeof(ControlType).GetField(controlTypeName)?.GetValue(null) as ControlType;
        }

        return null;
    }

    public static string? GetCallingProperty()
    {
        var stackTrace = new StackTrace();

        for (var i = 1; i <= MaxStackTraceLookUp; i++)
        {
            var callingProperty = stackTrace.GetFrame(i)?.GetMethod()?.Name;
            if (callingProperty is null)
            {
                continue;
            }

            if (callingProperty.StartsWith("get_") || callingProperty.StartsWith("set_"))
            {
                return callingProperty.Replace("get_", string.Empty)
                    .Replace("set_", string.Empty);
            }
        }

        return null;
    }

    public static object? WrapAutomationObject(Type type, object value)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(value);

        if (value.GetType() == type)
        {
            return value;
        }

#pragma warning disable IDISP001 // Dispose created
        var typeFactory = value.GetTypeFactory();
#pragma warning restore IDISP001 // Dispose created

        if (typeof(AutomationBase).IsAssignableFrom(type))
        {
            return typeFactory.CreateInstanceWithParametersAndAutoCompletion(type, value);
        }

        if (type == typeof(AutomationElement))
        {
            return value;
        }

        var collectionElementType = type.GetAnyElementType();
        if (collectionElementType is null)
        {
            return null;
        }

        if (value is not IEnumerable<AutomationElement> valueElements)
        {
            return null;
        }

        if (typeFactory.CreateInstanceWithParametersAndAutoCompletion(type) is not IList elementCollection)
        {
            return null;
        }

        if (typeof(AutomationBase).IsAssignableFrom(collectionElementType))
        {
            foreach (var automationElement in valueElements)
            {
                elementCollection.Add(typeFactory.CreateInstanceWithParametersAndAutoCompletion(collectionElementType, automationElement));
            }

            return elementCollection;
        }

        if (type == typeof(AutomationElement))
        {
            foreach (var automationElement in valueElements)
            {
                elementCollection.Add(automationElement);
            }

            return elementCollection;
        }

        return null;
    }
}
