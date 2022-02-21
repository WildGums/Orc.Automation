namespace Orc.Automation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;
    using System.Windows.Controls;

    [AutomatedControl(ControlTypeName = nameof(ControlType.List))]
    public class List : FrameworkElement
    {
        public List(AutomationElement element) 
            : base(element, ControlType.List)
        {
        }

        public IReadOnlyList<ListItem> Items => By.Many<ListItem>();

        public IReadOnlyList<TItem> GetItemsOfType<TItem>() => By.Many<TItem>();

        public bool CanSelectMultiply => Element.CanSelectMultiple();

        public AutomationElement SelectedItem
        {
            get => Element.GetSelection()?.FirstOrDefault();
            set => value?.Select();
        }

        public AutomationElement Select(int index)
        {
            return Element.TrySelectItem(index, out var element) ? element : null;
        }
    }
}
