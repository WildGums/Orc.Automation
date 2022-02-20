namespace Orc.Automation.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Automation;

    [AutomatedControl(ControlTypeName = nameof(ControlType.Tree))]
    public class Tree : FrameworkElement
    {
        public Tree(AutomationElement element)
            : base(element, ControlType.Tree)
        {
        }

        public IReadOnlyList<TreeItem> ChildItems => (IReadOnlyList<TreeItem>)By.Scope(TreeScope.Children).Many<TreeItem>() ?? Array.Empty<TreeItem>();
    }
}
