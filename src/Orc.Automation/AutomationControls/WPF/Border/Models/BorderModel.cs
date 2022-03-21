namespace Orc.Automation
{
    using System.Windows;
    using System.Windows.Media;

    [ActiveAutomationModel]
    public class BorderModel : FrameworkElementModel
    {
        public BorderModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public SolidColorBrush Background { get; set; }
        public SolidColorBrush BorderBrush { get; set; }
        public Thickness BorderThickness { get; set; }
        public CornerRadius CornerRadius { get; set; }
        public Thickness Padding { get; set; }
    }
}
