namespace Orc.Automation
{
    using System;
    using System.Collections.Generic;
    using Catel.Reflection;

    public static class ByExtensions
    {
        public static By Id(this By by)
        {
            ArgumentNullException.ThrowIfNull(by);

            var property = AutomationHelper.GetCallingProperty();
            if (property is null)
            {
                throw new AutomationException("Can't find calling property");
            }

            return by.Id(property);
        }

        public static By Name(this By by)
        {
            ArgumentNullException.ThrowIfNull(by);

            var property = AutomationHelper.GetCallingProperty();
            if (property is null)
            {
                throw new AutomationException("Can't find calling property");
            }

            return by.Name(property);
        }

        public static T? One<T>(this By by)
            where T : AutomationControl
        {
            ArgumentNullException.ThrowIfNull(by);

            PrepareSearch<T>(by);

            var type = typeof(T);
            var isRawControl = type.IsDecoratedWithAttribute<RawAttribute>();

            if (isRawControl)
            {
                return by.Part<T>();
            }
            
            var result = by.One();
            if (result is not null)
            {
                return (T)AutomationHelper.WrapAutomationObject(type, result);
            }

            return null; //by.Part<T>();
        }

        public static List<T> Many<T>(this By by)
        {
            ArgumentNullException.ThrowIfNull(by);

            PrepareSearch<T>(by);

            var result = by.Many();
            if (result is null)
            {
                return default;
            }

            return (List<T>)AutomationHelper.WrapAutomationObject(typeof(List<T>), result);
        }

        private static void PrepareSearch<T>(By by)
        {
            var type = typeof(T);

            var controlType = AutomationHelper.GetControlType(type);
            if (controlType is not null)
            {
                by.ControlType(controlType);
            }

            var className = AutomationHelper.GetControlClassName(type);
            if (!string.IsNullOrWhiteSpace(className))
            {
                by.ClassName(className);
            }
        }
    }
}
