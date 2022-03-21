namespace Orc.Automation
{
    using System.Windows;
    using System.Windows.Media;

    [ActiveAutomationModel]
    public class ControlModel : FrameworkElementModel
    {
        public ControlModel(AutomationElementAccessor accessor) 
            : base(accessor)
        {
        }

        public SolidColorBrush Background { get; set; }
        public SolidColorBrush BorderBrush { get; set; }
        public Thickness BorderThickness { get; set; }
        public FontFamily FontFamily { get; set; }
        public double FontSize { get; set; }
        public FontStretch FontStretch { get; set; }
        public FontStyle FontStyle { get; set; }
        public FontWeight FontWeight { get; set; }
        public SolidColorBrush Foreground { get; set; }
        public HorizontalAlignment HorizontalContentAlignment { get; set; }
        public bool IsTabStop { get; set; }
        public Thickness Padding { get; set; }
        public int TabIndex { get; set; }
        public VerticalAlignment VerticalContentAlignment { get; set; }
    }
}
