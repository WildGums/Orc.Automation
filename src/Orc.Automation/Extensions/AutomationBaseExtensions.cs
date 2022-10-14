namespace Orc.Automation
{
    using System;
    using Catel.Reflection;

    public static class AutomationBaseExtensions
    {
        public static bool IsVisible(this AutomationBase element)
        {
            ArgumentNullException.ThrowIfNull(element);

            //Automation can't find element if it's not visible, so no checks for null
            return element is not null && element.Element.IsVisible();
        }

        public static TValue? GetMapValue<TValue>(this AutomationBase? automation, object? source, string propertyName)
        {
            // We allow null here so we can use code like this:
            // Map.GetValue<TValue>(source, property);
            if (automation is null)
            {
                return default;
            }

            if (source is null)
            {
                return default;
            }

            return PropertyHelper.GetPropertyValue<TValue>(source, propertyName);
        }

        public static void SetMapValue<TValue>(this AutomationBase? automation, object? source, string propertyName, TValue value)
        {
            // We allow null here so we can use code like this:
            // Map.SetValue<TValue>(source, property, value);
            if (automation is null)
            {
                return;
            }

            if (source is null)
            {
                return;
            }

            PropertyHelper.SetPropertyValue(source, propertyName, value);
        }
    }
}
