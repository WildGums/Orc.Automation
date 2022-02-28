namespace Orc.Automation
{
    using System.Collections;
    using System.Windows.Controls;

    [AutomationAccessType]
    public class ItemsControlModel : FrameworkElementModel
    {
        public ItemsControlModel(AutomationElementAccessor accessor)
            : base(accessor)
        {
        }

        public ItemsPanelTemplate ItemsPanel { get; set; }
        public IEnumerable Items { get; set; }
    }
}
