namespace Orc.Automation;

using System;
using System.Collections.Generic;
using Catel.Data;
using Catel.Reflection;

public class AutomationControlModel : ModelBase
{
    protected readonly AutomationElementAccessor _accessor;

    private readonly HashSet<string> _ignoredProperties = new()
    {
        nameof(ModelBase.IsReadOnly),
        nameof(ModelBase.IsDirty)
    };

    public AutomationControlModel(AutomationElementAccessor accessor)
    {
        _accessor = accessor;
    }

    protected override T GetValueFromPropertyBag<T>(string propertyName)
    {
        if (_accessor is null || _ignoredProperties.Contains(propertyName))
        {
            return base.GetValueFromPropertyBag<T>(propertyName);
        }

        var (apiPropertyName, ownerType) = GetApiPropertyDescription(propertyName);
        
        return !string.IsNullOrWhiteSpace(apiPropertyName) 
            ? _accessor.GetValue<T>(apiPropertyName, ownerType)
            : base.GetValueFromPropertyBag<T>(propertyName);
    }

    protected override void SetValueToPropertyBag<TValue>(string propertyName, TValue value)
    {
        if (_accessor is null || _ignoredProperties.Contains(propertyName))
        {
            base.SetValueToPropertyBag(propertyName, value);

            return;
        }

        var (apiPropertyName, ownerType) = GetApiPropertyDescription(propertyName);
        if (!string.IsNullOrWhiteSpace(apiPropertyName))
        {
            _accessor.SetValue(apiPropertyName, value, ownerType);

            return;
        }

        base.SetValueToPropertyBag(propertyName, value);
    }

    private (string propertyName, Type type) GetApiPropertyDescription(string propertyName)
    {
        var property = PropertyHelper.GetPropertyInfo(this, propertyName);
        var apiAttribute = property.GetAttribute<ActiveAutomationPropertyAttribute>();
        if (apiAttribute is null)
        {
            var automationAccessTypeAttribute = GetType().GetAttribute<ActiveAutomationModelAttribute>();
            return automationAccessTypeAttribute is not null
                ? new ValueTuple<string, Type>(propertyName, automationAccessTypeAttribute.DefaultOwnerType)
                : default;
        }

        propertyName = string.IsNullOrWhiteSpace(apiAttribute.OriginalName) ? propertyName : apiAttribute.OriginalName;
        var type = apiAttribute.OwnerType;

        return new ValueTuple<string, Type>(propertyName, type);
    }
}
