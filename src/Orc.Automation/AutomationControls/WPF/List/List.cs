namespace Orc.Automation.Controls
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;


    [AutomatedControl(ControlTypeName = nameof(ControlType.DataGrid), ClassName = "ListView")]
    public class GridList : List
    {
        public GridList(AutomationElement element)
            : base(element, ControlType.DataGrid)
        {
        }
    }

    [AutomatedControl(ControlTypeName = nameof(ControlType.List), ClassName = "ListBox")]
    public class ListBox : List
    {
        public ListBox(AutomationElement element)
            : base(element)
        {
        }
    }

    [AutomatedControl(ControlTypeName = nameof(ControlType.List), ClassName = "ListView")]
    public class List : FrameworkElement<ListModel>
    {
        public List(AutomationElement element) 
            : base(element, ControlType.List)
        {
        }

        protected List(AutomationElement element, ControlType controlType)
            : base(element, controlType)
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
