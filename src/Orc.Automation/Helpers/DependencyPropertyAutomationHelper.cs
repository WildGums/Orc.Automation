namespace Orc.Automation
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Threading;
    using Catel;

    public static class DependencyPropertyAutomationHelper
    {
        public static bool TryGetDependencyPropertyValue(DependencyObject element, string propertyName, out object propertyValue)
        {
            Argument.IsNotNull(() => element);

            return TryGetDependencyPropertyValue(element, element.GetType(), propertyName, out propertyValue);
        }

        public static bool TryGetDependencyPropertyValue(DependencyObject element, Type ownerType, string propertyName, out object propertyValue)
        {
            Argument.IsNotNull(() => element);

            propertyValue = null;

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return false;
            }

            var dependencyProperty = GetDependencyPropertyByName(element, ownerType, propertyName);
            if (dependencyProperty is null)
            {
                return false;
            }

            var dependencyPropertyValue = element.GetValue(dependencyProperty);

            //TODO:Vladimir:find better way to do this
            if (dependencyPropertyValue is IEnumerable enumerableValue && dependencyPropertyValue.GetType().Name.StartsWith("IEnumerable"))
            {
                dependencyPropertyValue = enumerableValue.OfType<string>().ToList();
            }

            propertyValue = dependencyPropertyValue;

            return true;
        }

        public static bool SetDependencyPropertyValue(DependencyObject element, string propertyName, object value)
        {
            Argument.IsNotNull(() => element);

            return SetDependencyPropertyValue(element, element.GetType(), propertyName, value);
        }

        public static bool SetDependencyPropertyValue(DependencyObject element, Type ownerType, string propertyName, object value)
        {
            Argument.IsNotNull(() => element);
            
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                return false;
            }

            var dependencyProperty = GetDependencyPropertyByName(element, ownerType, propertyName);
            if (dependencyProperty is null)
            {
                return false;
            }

            element.SetCurrentValue(dependencyProperty, value);

            //Do events
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(delegate { }));

            return true;
        }

        private static DependencyProperty GetDependencyPropertyByName(DependencyObject dependencyObject, Type ownerType, string propertyName)
        {
            return GetDependencyPropertyByName(dependencyObject.GetType(), ownerType, propertyName);
        }

        private static DependencyProperty GetDependencyPropertyByName(Type dependencyObjectType, Type ownerType, string propertyName)
        {
            var dependencyProperty = DependencyPropertyDescriptor.FromName(propertyName, ownerType, dependencyObjectType)?.DependencyProperty;
            return dependencyProperty;
        }
    }
}
