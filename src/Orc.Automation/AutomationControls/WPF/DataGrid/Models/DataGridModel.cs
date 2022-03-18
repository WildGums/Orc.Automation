namespace Orc.Automation
{
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
            get => _accessor.ExecuteAutomationMethod<GetItemSourceAutomationMethodRun>() as IEnumerable;
        }
    }
}
