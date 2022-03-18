namespace Orc.Automation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    public class GetItemSourceAutomationMethodRun : NamedAutomationMethodRun
    {
        public override bool TryInvoke(FrameworkElement owner, AutomationMethod method, out AutomationValue result)
        {
            result = AutomationValue.FromValue(true);

            if (owner is not System.Windows.Controls.DataGrid dataGrid)
            {
                return false;
            }

            var itemSource = dataGrid.ItemsSource?.OfType<object>().ToList() ?? new List<object>();

            result = AutomationValue.FromValue(itemSource);
            return true;
        }
    }
}
