namespace Orc.Automation
{
    using System;
    using System.Linq;
    using System.Windows.Automation;
    using Catel;
    using Controls;

    public static class ComboboxExtensions
    {
        public static void Select(this ComboBox combobox, Func<AutomationElement, bool> predicate)
        {
            Argument.IsNotNull(() => combobox);

            combobox.InvokeInExpandState(() =>
            {
                var items = combobox.Items;

                var itemToSelect = items?.FirstOrDefault(predicate);
                itemToSelect?.Select();
            });
        }
    }
}
