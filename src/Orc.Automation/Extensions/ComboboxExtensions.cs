namespace Orc.Automation
{
    using System;
    using System.Linq;
    using System.Windows.Automation;
    using Controls;
    using FilterBuilder.Automation;

    public static class ComboboxExtensions
    {
        public static TValue? GetSelectedValue<TValue>(this ComboBox combobox)
        {
            ArgumentNullException.ThrowIfNull(combobox);

            if (typeof(TValue) == typeof(bool))
            {
                return (TValue?)combobox.GetSelectedBooleanValue();
            }

            if (typeof(TValue).IsEnum)
            {
                return (TValue?)combobox.GetSelectedEnumValue<TValue>();
            }

            return (TValue?)combobox.GetSelectedValue();
        }

        public static object? GetSelectedValue(this ComboBox combobox)
        {
            ArgumentNullException.ThrowIfNull(combobox);

            //TODO:Vladimir
            return combobox.SelectedItem?.As<FrameworkElement>()?.Current?.DataContext;
        }

        private static object? GetSelectedBooleanValue(this ComboBox combobox)
        {
            ArgumentNullException.ThrowIfNull(combobox);

            var itemText = combobox.SelectedItem?.TryGetDisplayText();
            if (Equals(itemText, "True"))
            {
                return true;
            }

            if (Equals(itemText, "False"))
            {
                return false;
            }

            return null;
        }

        private static object? GetSelectedEnumValue<TEnum>(this ComboBox combobox)
        {
            ArgumentNullException.ThrowIfNull(combobox);

            var itemText = combobox.SelectedItem?.TryGetDisplayText();

            return Enum.GetValues(typeof(TEnum))
                .OfType<TEnum>()
                .FirstOrDefault(value => Equals(value.ToDisplayString(), itemText));
        }

        public static void SelectValue(this ComboBox combobox, object? value)
        {
            ArgumentNullException.ThrowIfNull(combobox);

            var valueString = value.ToDisplayString();

            combobox.Select(x => Equals(x.TryGetDisplayText(), valueString));
        }

        public static void Select(this ComboBox combobox, Func<AutomationElement, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(combobox);
            ArgumentNullException.ThrowIfNull(predicate);

            combobox.InvokeInExpandState(() =>
            {
                var items = combobox.Items;

                var itemToSelect = items?.FirstOrDefault(predicate);
                itemToSelect?.Select();
            });
        }
    }
}
