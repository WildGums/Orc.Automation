namespace Orc.Automation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.TreeItem))]
    public class TreeItem : FrameworkElement
    {
        public TreeItem(AutomationElement element)
            : base(element, ControlType.TreeItem)
        {
        }

        public bool IsExpanded
        {
            get => Element.GetIsExpanded();
            set => Element.SetIsExpanded(value);
        }

        public string Header => By.Raw().One<Text>()?.Value;

        public IReadOnlyList<AutomationElement> ChildItems4
            => (IReadOnlyList<AutomationElement>)By
                .Scope(TreeScope.Children)
                .ControlType(ControlType.TreeItem)
                .Many();

        public IReadOnlyList<TreeItem> ChildItems5
            => (IReadOnlyList<TreeItem>)Element.GetChildElements()
                .Where(x => Equals(x.Current.ControlType, ControlType.TreeItem))
                .Select(x => new TreeItem(x))
                .ToList();

        public IReadOnlyList<TreeItem> ChildItems3
            => (IReadOnlyList<TreeItem>)Element.GetChildElements()
                .Where(x => Equals(x.Current.ControlType, ControlType.TreeItem))
                .Select(x => x.As<TreeItem>())
                .ToList();

        public IReadOnlyList<AutomationElement> ChildItems2
            => (IReadOnlyList<AutomationElement>) Element.GetChildElements()
                .Where(x => Equals(x.Current.ControlType, ControlType.TreeItem))
                .ToList();

        public IReadOnlyList<TreeItem> ChildItems => (IReadOnlyList<TreeItem>) By.Scope(TreeScope.Children).Many<TreeItem>() ?? Array.Empty<TreeItem>();
    }
}
