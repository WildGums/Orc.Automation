namespace Orc.Automation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Automation;

    [Control(ControlTypeName = nameof(ControlType.TreeItem))]
    public class TreeItem : FrameworkElement<TreeItemModel>
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

        public IReadOnlyList<TreeItem> ChildItems => (IReadOnlyList<TreeItem>) By.Scope(TreeScope.Children)
            .Many<TreeItem>() ?? Array.Empty<TreeItem>();
    }
}
