namespace Orc.Automation.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.List))]
    public class List : FrameworkElement<ListModel>
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
