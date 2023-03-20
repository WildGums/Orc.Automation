namespace Orc.Automation;

using System;
using System.Windows.Automation;
using System.Windows.Controls;

public class GotoVirtualizedItemAutomationMethodRun : NamedAutomationMethodRun
{
    public override bool TryInvoke(System.Windows.FrameworkElement owner, AutomationMethod method, out AutomationValue result)
    {
        result = AutomationValue.FromValue(string.Empty);

        var itemIndex = (int)method.Parameters[0].ExtractValue();
        if (owner is not ListBox listBox)
        {
            return false;
        }

        var item = listBox.Items[itemIndex];
        listBox.ScrollIntoView(item);
        listBox.UpdateLayout();

        if (listBox.ItemContainerGenerator.ContainerFromIndex(itemIndex) is not ListBoxItem itemContainer)
        {
            return false;
        }

        var id = (string)itemContainer.GetValue(AutomationProperties.AutomationIdProperty);
        if (string.IsNullOrWhiteSpace(id))
        {
            id = Guid.NewGuid().ToString();
            itemContainer.SetCurrentValue(AutomationProperties.AutomationIdProperty, id);
        }

        result = AutomationValue.FromValue(id);

        return true;
    }
}