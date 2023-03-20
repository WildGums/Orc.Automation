namespace Orc.Automation;

using System.Windows;
using System.Windows.Media;

public class TextModel : FrameworkElementModel
{
    public TextModel(AutomationElementAccessor accessor) 
        : base(accessor)
    {
    }

    public string Text { get; set; }
    public SolidColorBrush Background { get; set; }
    public SolidColorBrush Foreground { get; set; }
    public FontStretch FontStretch { get; set; }
    public double BaselineOffset { get; set; }
    public LineBreakCondition BreakAfter { get; set; }
    public LineBreakCondition BreakBefore { get; set; }
    public FontFamily FontFamily { get; set; }
    public TextWrapping TextWrapping { get; set; }
    public TextTrimming TextTrimming { get; set; }
    public TextAlignment TextAlignment { get; set; }
    public Thickness Padding { get; set; }
    public double LineHeight { get; set; }
    public bool IsHyphenationEnabled { get; set; }
    public FontStyle FontStyle { get; set; }
    public FontWeight FontWeight { get; set; }
    public double FontSize { get; set; }
}