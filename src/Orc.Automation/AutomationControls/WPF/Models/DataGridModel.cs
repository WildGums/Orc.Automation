namespace Orc.Automation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    public class DataGridModel : FrameworkElementModel
    {
        public DataGridModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        [ApiProperty]
        public double ColumnHeaderHeight { get; set; }

        //TODO:
        public IEnumerable ItemsSource
        {
            get => _accessor.ExecuteAutomationMethod<GetItemSourceMethod>() as IEnumerable;
        }
    }

    public class GetItemSourceMethod : NamedAutomationMethodRun
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
