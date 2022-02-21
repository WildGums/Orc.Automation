namespace Orc.Automation;

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

        var apiPropertyName = GetApiPropertyName(propertyName);
        if (!string.IsNullOrWhiteSpace(apiPropertyName))
        {
            return _accessor.GetValue<T>(apiPropertyName);
        }

        return base.GetValueFromPropertyBag<T>(propertyName);
    }

    protected override void SetValueToPropertyBag<TValue>(string propertyName, TValue value)
    {
        if (_accessor is null || _ignoredProperties.Contains(propertyName))
        {
            base.SetValueToPropertyBag(propertyName, value);

            return;
        }

        var apiPropertyName = GetApiPropertyName(propertyName);
        if (!string.IsNullOrWhiteSpace(apiPropertyName))
        {
            _accessor.SetValue(apiPropertyName, value);

            return;
        }

        base.SetValueToPropertyBag(propertyName, value);
    }

    private string GetApiPropertyName(string propertyName)
    {
        var property = PropertyHelper.GetPropertyInfo(this, propertyName);
        var apiAttribute = property.GetAttribute<ApiPropertyAttribute>();
        if (apiAttribute is null)
        {
            return GetType().IsDecoratedWithAttribute<AutomationAccessType>() ? propertyName : null;
        }

        return string.IsNullOrWhiteSpace(apiAttribute.OriginalName) ? propertyName : apiAttribute.OriginalName;
    }
}
