namespace Orc.Automation
{
    using System.Collections;

    public class DataGridModel : FrameworkElementModel
    {
        public DataGridModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        [ActiveAutomationProperty]
        public double ColumnHeaderHeight { get; set; }

        //TODO:
        public IEnumerable ItemsSource
        {
            get => _accessor.ExecuteAutomationMethod<GetItemSourceAutomationMethodRun>() as IEnumerable;
        }
    }
}
