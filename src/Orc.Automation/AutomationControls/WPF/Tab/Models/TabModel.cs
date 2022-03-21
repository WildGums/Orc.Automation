namespace Orc.Automation
{
    using System.Windows.Controls;

    [ActiveAutomationModel]
    public class TabModel : SelectorModel
    {
        public TabModel(AutomationElementAccessor accessor)
            : base(accessor)
        {
        }

        public Dock TabStripPlacement { get; set; }
        public string SelectedContentStringFormat => _accessor.GetValue<string>();
        public object SelectedContent { get; set; }
        public string ContentStringFormat { get; set; }
    }
}
